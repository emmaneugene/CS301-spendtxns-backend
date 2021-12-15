using System.Linq;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using CS301_Spend_Transactions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Repo.Helpers
{
    public class GroupMappingHelper : IGroupMappingHelper
    {
        private readonly ILogger<UserService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public GroupMappingHelper(IServiceScopeFactory scopeFactory,
            ILogger<UserService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        
        // Currently just taking every group from database and iterate
        // Won't affect the performance since the number of groups are limited
        public string GetGroupFromMCC(int MCC)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return dbContext.Groups.First(groups => groups.MinMCC < MCC && groups.MaxMCC > MCC).Name;
        }
    }
}