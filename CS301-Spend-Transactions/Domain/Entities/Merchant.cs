using System;
using System.Collections.Generic;

namespace CS301_Spend_Transactions.Models
{
    public class Merchant
    {
        // Primary key
        public string Name { get; set; }
        public int? MCC { get; set; }
        
        // Navigation properties
        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}