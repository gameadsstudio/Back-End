using System;
using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Models.User;

namespace GAME_ADS_STUDIO_API.Data.User
{
    public class UserRepository : IUserRepository
    {

        public IEnumerable<UserModel> GetUsers()
        {
            var users = new List<UserModel>
            {
                new UserModel{Id=0, Username="bastien.roumanteau@epitech.eu", Firstname="Bastien", Lastname="Roumanteau"},
                new UserModel{Id=1, Username="antoine.ducrot@epitech.eu", Firstname="Antoine", Lastname="Ducrot"},
                new UserModel{Id=2, Username="louis-albert.bui@epitech.eu", Firstname="Louis-Albert", Lastname="Bui"},
                new UserModel{Id=3, Username="cédric.hennequin@epitech.eu", Firstname="Cédric", Lastname="Hennequin"},

            };

            return users;
        }

        public UserModel GetUserById(int id)
        {
            throw new NotImplementedException();
        }
        
    }
}
