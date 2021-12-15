using System.Linq;
using System.Transactions;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Repo.Helpers
{
    public class FailedTransactionErrorHelper : IFailedTransactionErrorHelper
    {
        private readonly ILogger<FailedTransactionErrorHelper> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FailedTransactionErrorHelper(IServiceScopeFactory scopeFactory,
            ILogger<FailedTransactionErrorHelper> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void HandleFailedTransaction(TransactionDTO transactionDto)
        {
            var failedTransaction =  PersistFailedTransaction(transactionDto);

            if (failedTransaction.Count > 2)
            {
                
            }
        }

        private FailedTransaction PersistFailedTransaction(TransactionDTO transactionDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var failedTransactions = dbContext.FailedTransactions.Where(
                t => t.Transaction_Id == transactionDto.Transaction_Id
            );

            FailedTransaction failedTransaction = null;

            if (failedTransactions.Any())
            {
                failedTransaction = failedTransactions.First();
            }
            else
            {
                failedTransaction = DtoToFailedTransaction(transactionDto);
            }

            failedTransaction.Count++;

            dbContext.FailedTransactions.Add(failedTransaction);
            dbContext.SaveChangesAsync();

            return failedTransaction;
        }

        private FailedTransaction DtoToFailedTransaction(TransactionDTO transactionDto)
        {
            return new FailedTransaction
            {
                Id = transactionDto.Id,
                Transaction_Id = transactionDto.Transaction_Id,
                Merchant = transactionDto.Merchant,
                MCC = transactionDto.MCC,
                Currency = transactionDto.Currency,
                Amount = transactionDto.Amount,
                Transaction_Date = transactionDto.Transaction_Date,
                Card_Id = transactionDto.Card_Id,
                Card_Pan = transactionDto.Card_Pan,
                Card_Type = transactionDto.Card_Type,
                Count = 0
            };
        }
    }
}