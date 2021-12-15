namespace CS301_Spend_Transactions.Controllers.Interfaces
{
    public interface IDatabaseController
    {
        void SeedUsers();

        void SeedCards();

        void SeedTransactions();

        void SeedGroups();
    }
}