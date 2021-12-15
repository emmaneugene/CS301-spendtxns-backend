using CS301_Spend_Transactions.Controllers.Interfaces;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Controllers
{
    public class CardController : BaseController<CardController>, ICardController
    {
        private readonly ILogger<CardController> _logger;
        private ICardService _cardService;

        public CardController(ILogger<CardController> logger, 
            ICardService cardService) : base(logger)
        {
            _logger = logger;
            _cardService = cardService;
        }
        
        [HttpGet("/api/Card/GetCard")]
        public Card GetCardById(string Id)
        {
            return _cardService.GetCardById(Id);
        }
    }
}