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
    /**
     * Helper class that helps to construct User and Card objects from UserDTO
     */
    public class UserMapperHelper
    {
        public static UserDTO ToUserDTO(string body)
        {
            return JsonConvert.DeserializeObject<UserDTO>(body);
        }

        public static User ToUser(UserDTO userDto)
        {
            return new User
            {
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNo = userDto.Phone,
                Email = userDto.Email,
                CreatedAt = userDto.CreatedAt,
                UpdatedAt = userDto.UpdatedAt
            };
        }

        public static Card ToCard(UserDTO userDto)
        {
            return new Card
            {
                Id = userDto.CardId,
                CardPan = userDto.CardPan,
                CardType = userDto.CardType,
                UserId = userDto.Id
            };
        }
    }
}