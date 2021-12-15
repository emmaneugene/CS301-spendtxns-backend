using System.Data;
using System.Linq;
using CS301_Spend_Transactions.Domain.DTO;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Repo.Helpers;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        // Manages the lifetime of the services we going to inject
        private readonly IServiceScopeFactory _scopeFactory;

        public UserService(IServiceScopeFactory scopeFactory,
            ILogger<UserService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        
        public User GetUserById(string Id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Using LINQ expressions here
            return dbContext.Users.First(user => user.Id == Id);
        }
        
        /**
         * Adds new User and Card objects into the database based on a UserDTO
         */
        public User AddUser(UserDTO userDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var user = UserMapperHelper.ToUser(userDto);
            var card = UserMapperHelper.ToCard(userDto);

            if (dbContext.Users.Find(user.Id) is null)
            {
                dbContext.Users.Add(user);    
            }
            
            dbContext.Cards.Add(card);
            
            dbContext.SaveChanges();
            return user;
        }
    }

}