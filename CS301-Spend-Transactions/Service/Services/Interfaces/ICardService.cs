using CS301_Spend_Transactions.Models;

namespace CS301_Spend_Transactions.Services.Interfaces
{
    public interface ICardService
    {
        Card GetCardById(string Id);
    }
}