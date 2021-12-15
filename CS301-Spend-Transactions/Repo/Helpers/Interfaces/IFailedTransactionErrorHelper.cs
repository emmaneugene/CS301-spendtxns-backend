using CS301_Spend_Transactions.Domain.DTO;

namespace CS301_Spend_Transactions.Repo.Helpers.Interfaces
{
    public interface IFailedTransactionErrorHelper
    {
        void HandleFailedTransaction(TransactionDTO transactionDto);
    }
}