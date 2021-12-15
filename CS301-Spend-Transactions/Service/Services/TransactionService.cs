using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Domain.Exceptions;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Repo.Helpers;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Services
{
    /**
     * Service that handles the insertion and relevant processing of transaction records
     */
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;

        // Manages the lifetime of the services we going to inject
        private readonly IServiceScopeFactory _scopeFactory;
        
        private IFailedTransactionErrorHelper _failedTransactionErrorHelper;


        public TransactionService(IServiceScopeFactory scopeFactory,
            ILogger<TransactionService> logger,
            IFailedTransactionErrorHelper failedTransactionErrorHelper)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _failedTransactionErrorHelper = failedTransactionErrorHelper;
        }

        /**
         * Adds a transaction into the database, handling all the necessary checks and points earned
         */
        public async Task<Transaction> AddTransaction(TransactionDTO transactionDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var transaction = TransactionMapperHelper.ToTransaction(transactionDto);

            // 1-Validate transaction
            if (transaction.Amount < 0)
            {
                _logger.LogCritical("Amount is negative");
                _failedTransactionErrorHelper.HandleFailedTransaction(transactionDto);
                throw new InvalidTransactionException("Transaction cannot have a negative amount");
            }

            if (dbContext.Cards.Find(transaction.CardId) is null)
            {
                _logger.LogCritical("Card not found");
                _failedTransactionErrorHelper.HandleFailedTransaction(transactionDto);
                throw new InvalidTransactionException("Invalid card ID in transaction record");
            }

            if (dbContext.Transactions.Find(transaction.Id) != null)
            {
                _logger.LogCritical("Transaction exists within the database");
                _failedTransactionErrorHelper.HandleFailedTransaction(transactionDto);
                throw new InvalidTransactionException("Transaction has already been processed");
            }

            // 2-Find any exclusions. If an exclusion applies, no points are earned
            var exclusions = dbContext.Exclusions.Where(exclusion
                => exclusion.MCC == transactionDto.MCC);

            if (exclusions.Any())
            {
                dbContext.Transactions.Add(transaction);
                dbContext.SaveChangesAsync();
                return transaction;
            }

            // 3-Check for points earned through card program
            var foreignSpend = (!transactionDto.Currency.Equals("SGD"));

            // Check for special program rules based on MCC 
            var programs = dbContext.Programs.Where(program =>
                program.CardType == transactionDto.Card_Type
                && program.MinSpend <= transactionDto.Amount
                && program.MaxSpend > transactionDto.Amount
                && program.ForeignSpend == foreignSpend
                && program.MCC == transactionDto.MCC
            );

            // Otherwise, use base rule
            if (!programs.Any())
            {
                programs = dbContext.Programs.Where(program =>
                    program.CardType == transactionDto.Card_Type
                    && program.MinSpend <= transactionDto.Amount
                    && program.MaxSpend > transactionDto.Amount
                    && program.ForeignSpend == foreignSpend
                    && program.MCC == -1
                );
            }

            if (programs.Any())
            {
                var program = programs.First();
                var points = new Points
                {
                    Amount = program.GetReward(transaction.Amount),
                    ProcessedDate = DateTime.Now,
                    TransactionId = transaction.Id,
                    PointsTypeId = program.PointsTypeId
                };

                // Handle foreign currency conversion
                if (foreignSpend)
                {
                    points.Amount = CurrencyConverter.ConvertToSgd(transactionDto.Currency, points.Amount);
                }

                dbContext.Points.Add(points);
            }


            // 4-Check for points earned through campaigns
            var campaigns = dbContext.Campaigns.Where(campaign =>
                campaign.CardType == transactionDto.Card_Type
                && campaign.MerchantName == transactionDto.Merchant
                && campaign.MinSpend <= transactionDto.Amount
                && campaign.MaxSpend > transactionDto.Amount
                && campaign.ForeignSpend == foreignSpend
                && campaign.StartDate < DateTime.Now
                && campaign.EndDate > DateTime.Now
            );

            foreach (var campaign in campaigns)
            {
                var points = new Points
                {
                    Amount = campaign.GetReward(transaction.Amount),
                    ProcessedDate = DateTime.Now,
                    TransactionId = transaction.Id,
                    PointsTypeId = campaign.PointsTypeId
                };

                // Handle foreign currency conversion
                if (foreignSpend)
                {
                    points.Amount = CurrencyConverter.ConvertToSgd(transactionDto.Currency, points.Amount);
                }

                dbContext.Points.Add(points);
            }

            // 5-Save changes to db
            dbContext.Transactions.Add(transaction);
            dbContext.SaveChangesAsync();
            return transaction;
        }

        public Transaction GetTransactionById(string id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Using LINQ expressions here
            return dbContext.Transactions.First(transaction => transaction.Id == id);
        }
    }
}