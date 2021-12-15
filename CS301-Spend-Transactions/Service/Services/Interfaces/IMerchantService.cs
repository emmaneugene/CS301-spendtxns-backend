using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;

namespace CS301_Spend_Transactions.Services.Interfaces
{
    public interface IMerchantService
    {
        Merchant AddMerchant(TransactionDTO transactionDto);

        Merchant GetMerchantByName(string name);
    }
}