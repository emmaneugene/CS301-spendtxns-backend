using System;
using System.Linq;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Services
{
    /**
     * Service that handles the insertion of new merchants in transactionDto records
     */
    public class MerchantService : IMerchantService
    {
        private readonly ILogger<MerchantService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public MerchantService(IServiceScopeFactory scopeFactory,
            ILogger<MerchantService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        
        /**
         * Adds a merchant into the database if it does not already exist
         * TODO: Can this be optimized so that it doesn't have to be called for each
         * transaction? Might be a performance bottleneck given transaction processing
         */
        public Merchant AddMerchant(TransactionDTO transactionDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var dbMerchant = dbContext.Merchants.Find(transactionDto.Merchant);


            if (dbMerchant != null)
            {
                return dbMerchant;
            }

            var merchant = new Merchant()
            {
                Name = transactionDto.Merchant,
                MCC = transactionDto.MCC
            };

            try
            {
                dbContext.Merchants.Add(merchant);
                dbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                _logger.LogWarning(e.Message);
            }
            
            return merchant;
        }

        public Merchant GetMerchantByName(string name)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return dbContext.Merchants.First(merchant => merchant.Name == name);
        }
    }
}