using System;
using System.Collections.Generic;

namespace CS301_Spend_Transactions.Models
{
    /**
     * Entity representing a User with basic details 
     */
    public class User
    {
        // Primary key
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<Card> Cards { get; set; }
    }
}