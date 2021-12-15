using System;
using System.Collections.Generic;
using CS301_Spend_Transactions.Models;

namespace CS301_Spend_Transactions.Models
{
    /**
     * Entity representing a Card, which references a User and a set of Rules that apply
     * to all transactions involving that Card
     */
    public class Card
    {   
        // Primary key
        public string Id { get; set; }
        public string CardPan { get; set; }
        public string CardType { get; set; }
        
        // Foreign key
        public string UserId { get; set; }
        
        // Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}