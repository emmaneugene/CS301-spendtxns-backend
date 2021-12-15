using System.Threading.Tasks;
using CS301_Spend_Transactions.Models;

using CS301_Spend_Transactions.Controllers.Interfaces;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Controllers
{
    public class TransactionController : BaseController<TransactionController>, ITransactionController
    {
        private readonly ILogger<TransactionController> _logger;
        private ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, 
            ITransactionService transactionService) : base(logger)
        {
            _logger = logger;
            _transactionService = transactionService;
        }
        
        [HttpPost("/api/Transaction/AddTransaction")]
        public Task<Transaction> AddTransaction(TransactionDTO transactionDto)
        {
            return _transactionService.AddTransaction(transactionDto);
        }

        [HttpGet("/api/Transaction/GetTransaction")]
        public Transaction GetTransactionById(string Id)
        {
            return _transactionService.GetTransactionById(Id);
        }

        
    }
}