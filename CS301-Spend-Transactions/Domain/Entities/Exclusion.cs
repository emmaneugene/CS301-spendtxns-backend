namespace CS301_Spend_Transactions.Models
{
    /**
     * Exclusions that apply to transactions based on CardType and MCC. If an
     * exclusion applies, the transaction does not earn any points
     */
    public class Exclusion
    {
        // Primary key
        public int Id { get; set; }
        public string CardType { get; set; }
        public int MCC { get; set; }
    }

}