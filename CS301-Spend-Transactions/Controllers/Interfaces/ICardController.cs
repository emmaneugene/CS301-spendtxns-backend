using CS301_Spend_Transactions.Models;

namespace CS301_Spend_Transactions.Controllers.Interfaces
{
    public interface ICardController
    {
        Card GetCardById(string Id);
    }
}