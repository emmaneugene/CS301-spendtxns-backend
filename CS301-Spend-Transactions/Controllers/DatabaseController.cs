using CS301_Spend_Transactions.Controllers.Interfaces;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Controllers
{
    public class DatabaseController : BaseController<DatabaseController>, IDatabaseController
    {
        private readonly ILogger<DatabaseController> _logger;
        private IDatabaseSeeder _databaseSeeder;
        
        public DatabaseController(ILogger<DatabaseController> logger, IDatabaseSeeder databaseSeeder) : base(logger)
        {
            _logger = logger;
            _databaseSeeder = databaseSeeder;
        }

        [HttpGet("/api/Database/InitialSeed")]
        public async void InitialSeed()
        {
            // commented out because this is too slow via seeding
            // import csv to database instead
            // await _databaseSeeder.SeedUserEntries();
            // await _databaseSeeder.SeedCardEntries();
            await _databaseSeeder.SeedMerchantEntries();
            await _databaseSeeder.SeedGroupEntries();
            await _databaseSeeder.SeedPointsTypeEntries();
            await _databaseSeeder.SeedProgramEntries();
            await _databaseSeeder.SeedExclusionEntries();
            await _databaseSeeder.SeedCampaignEntries();
            await _databaseSeeder.SeedProgramEntries();
            // await _databaseSeeder.SeedTransactionEntries();
        }

        [HttpGet("/api/Database/SeedUsers")]
        public async void SeedUsers()
        {
            await _databaseSeeder.SeedUserEntries();
        }

        [HttpGet("/api/Database/SeedCards")]
        public async void SeedCards()
        {
            await _databaseSeeder.SeedCardEntries();
        }
        
        [HttpGet("/api/Database/SeedUsersAndCards")]
        public async void SeedUsersAndCards()
        {
            await _databaseSeeder.SeedUserAndCardEntries();
        }

        [HttpGet("/api/Database/SeedTransactions")]
        public async void SeedTransactions()
        {
            await _databaseSeeder.SeedTransactionEntries();
        }
        
        [HttpGet("/api/Database/SeedGroups")]
        public async void SeedGroups()
        {
            await _databaseSeeder.SeedGroupEntries();
        }
        
        [HttpGet("/api/Database/SeedPrograms")]
        public async void SeedPrograms()
        {
            await _databaseSeeder.SeedProgramEntries();
        }
        
        [HttpGet("/api/Database/SeedCampaigns")]
        public async void SeedCampaigns()
        {
            await _databaseSeeder.SeedCampaignEntries();
        }
        
        [HttpGet("/api/Database/SeedExclusions")]
        public async void SeedExclusions()
        {
            await _databaseSeeder.SeedExclusionEntries();
        }
        
        [HttpGet("/api/Database/SeedMerchants")]
        public async void SeedMerchants()
        {
            await _databaseSeeder.SeedMerchantEntries();
        }
    }
}