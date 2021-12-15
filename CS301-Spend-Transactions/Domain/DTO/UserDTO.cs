using System;

namespace CS301_Spend_Transactions.Domain.DTO
{
    /**
     * Data Transfer Object (DTO) corresponding to fields in users.csv entries
     */
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CardId { get; set; }
        public string CardPan { get; set; }
        public string CardType { get; set; }
    }
}