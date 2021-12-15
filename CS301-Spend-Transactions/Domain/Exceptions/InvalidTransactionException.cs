using System;

namespace CS301_Spend_Transactions.Domain.Exceptions
{
    public class InvalidTransactionException : Exception
    {
        public InvalidTransactionException(string message)
        {
        }
    }
}