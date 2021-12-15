using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Controllers
{
    public class SQSController : BaseController<SQSController>
    {
        private readonly ILogger<SQSController> _logger;
        private ISQSService _sqsService;
        
        public SQSController(ILogger<SQSController> logger, ISQSService sqsService) : base(logger)
        {
            _logger = logger;
            _sqsService = sqsService;
        }
        
        [HttpGet("/api/SQS/GetMessages")]
        public async Task<List<Message>> GetMessages()
        {
            return await _sqsService.GetMessages();
        }
    }
}