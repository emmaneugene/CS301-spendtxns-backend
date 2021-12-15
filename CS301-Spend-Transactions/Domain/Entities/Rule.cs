using System;
using System.Collections.Generic;

/**
 * Create subtypes of Rule class with entity type hierarchy mapping 
 * (https://docs.microsoft.com/en-us/ef/core/modeling/inheritance)
 */
namespace CS301_Spend_Transactions.Models
{
    /**
     * A Rule that applies to every Transaction of a given CardType. Rule is an
     * abstract class that can be extended based on different use cases
     */
    public abstract class Rule
    {
        // Primary key
        public int Id { get; set; }
        public string CardType { get; set; }
        public decimal MinSpend { get; set; }
        public decimal MaxSpend { get; set; }
        public bool ForeignSpend { get; set; }
        public decimal Multiplier { get; set; }
        
        // Foreign key
        public int PointsTypeId { get; set; } 
        // Navigation properties
        public virtual PointsType PointsType { get; set; }
        
        public abstract decimal GetReward(decimal amount);
    }

    /**
     * Programs are the default earning scheme for a cards, and are associated with a type of point
     */
    public class Program : Rule
    {
        public int MCC { get; set; }
        
        public override decimal GetReward(decimal amount)
        {
            return Multiplier * amount;
        }
    }
    
    /**
     * Campaigns are short-term spending bonuses that can be implemented by banks in collaboration
     * with merchants. Their rewards are also associated with point types
     */
    public class Campaign : Rule
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        // Foreign key
        public string MerchantName { get; set; }
        
        // Navigation properties
        public Merchant Merchant { get; set; }
        
        public override decimal GetReward(decimal amount)
        {
            return Multiplier * amount;
        }
    }
}