using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace CS301_Spend_Transactions.Services.Interfaces
{
    public interface ISQSService
    {
        Task<List<Message>> GetMessages();

        Task<Message> GetSingleMessage();
    }
}