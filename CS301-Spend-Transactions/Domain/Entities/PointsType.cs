using System;
using System.Collections;
using System.Collections.Generic;

namespace CS301_Spend_Transactions.Models
{
    /**
     * Stores different categories of points that can be earned from card transactions
     * (e.g. Shopping, Miles, Cashback)
     */
    public class PointsType
    {
        // Primary key
        public int Id { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        
        // Navigation properties
        public ICollection<Points> CreditedPoints { get; set; }
        
        public ICollection<Rule> Rules { get; set; }
    }
}