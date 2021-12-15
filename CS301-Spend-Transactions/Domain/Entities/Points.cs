using System;
using System.Collections.Generic;

namespace CS301_Spend_Transactions.Models
{
    public class Points
    {
        // Primary key
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        
        // Foreign key
        public string TransactionId { get; set; } // references Transaction table
        public int PointsTypeId { get; set; } // references PointsType table  
        
        // Navigation properties
        public virtual Transaction Transaction { get; set; }
        public virtual PointsType PointsType { get; set; }
        
    }
}