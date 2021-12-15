using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;
using Microsoft.AspNetCore.Mvc;

namespace CS301_Spend_Transactions.Controllers.Interfaces
{
    public interface IUserController
    {
        User AddUser(UserDTO userDto);

        User GetUserById(string Id);
        
        
    }
}