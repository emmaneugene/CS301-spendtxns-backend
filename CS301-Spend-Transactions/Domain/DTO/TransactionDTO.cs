using System;

namespace CS301_Spend_Transactions.Domain.DTO
{
    /**
     * Data Transfer Object (DTO) corresponding to fields in spend.csv entries
     */
    public class TransactionDTO
    {
        public string Id { get; set; }
        public string Transaction_Id { get; set; }
        public string Merchant { get; set; }
        public int? MCC { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public DateTime Transaction_Date { get; set; }
        public string Card_Id { get; set; }
        
        public string Card_Pan { get; set; }
        public string Card_Type { get; set; }
    }
}