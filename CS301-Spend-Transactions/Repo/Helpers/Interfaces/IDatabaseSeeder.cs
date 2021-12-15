using System.Threading.Tasks;

namespace CS301_Spend_Transactions.Repo.Helpers.Interfaces
{
    public interface IDatabaseSeeder
    {
        Task<int> SeedUserEntries();
        Task<int> SeedDummyUserEntries();

        Task<int> SeedCardEntries();
        Task<int> SeedDummyCardEntries();

        Task<int> SeedUserAndCardEntries();
        
        Task<int> SeedTransactionEntries();

        Task<int> SeedGroupEntries();
        
        Task<int> SeedPointsTypeEntries();

        Task<int> SeedProgramEntries();

        Task<int> SeedExclusionEntries();

        Task<int> SeedCampaignEntries();    

        Task<int> SeedMerchantEntries();
    }
}