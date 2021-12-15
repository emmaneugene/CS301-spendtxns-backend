using System.Threading.Tasks;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;

namespace CS301_Spend_Transactions.Services.Interfaces
{
    public interface IUserService
    {
        User AddUser(UserDTO userDto);

        User GetUserById(string Id);
    }
}