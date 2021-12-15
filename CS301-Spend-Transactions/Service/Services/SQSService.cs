using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using CS301_Spend_Transactions.Repo.Helpers;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.BC;

namespace CS301_Spend_Transactions.Services
{
    public class SQSService : ISQSService
    {
        private readonly ILogger<SQSService> _logger;
        private readonly ISQSHelper _sqsHelper;

        public SQSService(ILogger<SQSService> logger, ISQSHelper sqsHelper)
        {
            _logger = logger;
            _sqsHelper = sqsHelper;
        }

        public async Task<List<Message>> GetMessages()
        {
            _logger.LogInformation(
                "[SQSService/GetMessages] awaiting SQSHelper");
            var messages = await _sqsHelper.GetMessage();

            return messages;
        }

        public async Task<Message> GetSingleMessage()
        {
            return await _sqsHelper.GetSingleMessage();
        }
    }
}