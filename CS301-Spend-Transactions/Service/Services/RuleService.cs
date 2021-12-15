using System.Linq;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Services
{
    public class RuleService : IRuleService
    {
        private readonly ILogger<RuleService> _logger;
        // Manages the lifetime of the services we going to inject
        private readonly IServiceScopeFactory _scopeFactory;
        
        public RuleService(IServiceScopeFactory scopeFactory,
            ILogger<RuleService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Rule GetRule(Card card, Transaction transaction)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return dbContext.Programs.Where(p => p.MinSpend < transaction.Amount && p.CardType == card.CardType)
                .OrderBy(p => p.MinSpend)
                .Last();
        }
    }
}