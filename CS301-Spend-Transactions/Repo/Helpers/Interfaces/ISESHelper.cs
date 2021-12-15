namespace CS301_Spend_Transactions.Repo.Helpers.Interfaces
{
    public interface ISESHelper
    {
        void SendFailedTransactionEmail(string transactionId);
    }
}