using System;
using System.Collections.Generic;

namespace CS301_Spend_Transactions.Models
{
    public class Transaction
    {
        // Primary key
        public string Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        // Foreign keys
        public string CardId { get; set; } // references card table
        public string MerchantName{ get; set; } // references merchant table
        
        // Navigation properties
        public Card Card { get; set; }
        public Merchant Merchant { get; set; }
        public ICollection<Points> AccumulatedPoints { get; set; }
        
    }
}