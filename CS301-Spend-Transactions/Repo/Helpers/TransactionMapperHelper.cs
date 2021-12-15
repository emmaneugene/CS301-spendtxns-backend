using System;
using System.Diagnostics;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CS301_Spend_Transactions.Repo.Helpers
{
    public static class TransactionMapperHelper
    {
        public static TransactionDTO ToTransactionDTO(string body)
        {
            return JsonConvert.DeserializeObject<TransactionDTO>(body);
        }

        public static Transaction ToTransaction(TransactionDTO transactionDTO)
        {
            return new Transaction
            {
                Id = transactionDTO.Transaction_Id,
                TransactionDate = transactionDTO.Transaction_Date,
                Currency = transactionDTO.Currency,
                Amount = transactionDTO.Amount,
                CardId = transactionDTO.Card_Id,
                MerchantName = transactionDTO.Merchant
            };
        }
    }
}