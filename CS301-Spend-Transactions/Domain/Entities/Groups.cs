namespace CS301_Spend_Transactions.Models
{
    /**
     * Groups are defined for merchants within a certain range of MCC codes
     */
    public class Groups
    {
        // Primary key
        public string Name { get; set; }
        public int MinMCC { get; set; }
        public int MaxMCC { get; set; }
        
    }
}