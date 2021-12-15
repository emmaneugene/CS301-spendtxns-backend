using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace CS301_Spend_Transactions.Repo.Helpers.Interfaces
{
    public interface ISQSHelper
    {
        Task<List<Message>> GetMessage();
        
        Task<Message> GetSingleMessage();
    }
}