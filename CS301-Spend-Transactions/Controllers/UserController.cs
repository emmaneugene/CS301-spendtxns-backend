using CS301_Spend_Transactions.Controllers.Interfaces;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Repo.Helpers;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Controllers
{
    public class UserController : BaseController<UserController>, IUserController
    {
        private readonly ILogger<UserController> _logger;
        private IUserService _userService;
        private IDatabaseSeeder _databaseSeeder;
        private ISESHelper _sesHelper;

        public UserController(ILogger<UserController> logger, 
            IUserService userService,
            IDatabaseSeeder databaseSeeder,
            ISESHelper sesHelper) : base(logger)
        {
            _logger = logger;
            _userService = userService;
            _databaseSeeder = databaseSeeder;
            _sesHelper = sesHelper;
        }
        
        [HttpPost("/api/User/AddUser")]
        public User AddUser(UserDTO userDto)
        {
            return _userService.AddUser(userDto);
        }



        [HttpGet("/api/User/GetUser")]
        public User GetUserById(string Id)
        {
            return _userService.GetUserById(Id);
        }

        [HttpGet("api/User/SendEmail")]
        public void SendEmail(string Id)
        {
            _sesHelper.SendFailedTransactionEmail(Id);
        }
    }
}