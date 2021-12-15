using System.Linq;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Services
{
    public class CardService : ICardService
    {
        private readonly ILogger<CardService> _logger;
        // Manages the lifetime of the services we going to inject
        private readonly IServiceScopeFactory _scopeFactory;
        
        public CardService(IServiceScopeFactory scopeFactory,
            ILogger<CardService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Card GetCardById(string Id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Using LINQ expressions here
            return dbContext.Cards.First(card => card.Id == Id);
        }
    }
}