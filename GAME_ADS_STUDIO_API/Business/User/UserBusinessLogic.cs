using GAME_ADS_STUDIO_API.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GAME_ADS_STUDIO_API.Configuration;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.Extensions.Options;
using GAME_ADS_STUDIO_API.Models.User;

namespace GAME_ADS_STUDIO_API.Business.User
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IUserRepository _repository;
        private readonly AppSettings _appSettings;

        public UserBusinessLogic(IOptions<AppSettings> appSettings)
        {
            //_repository = new UserRepository(context);
            _appSettings = appSettings.Value;
        }

        public UserModel AddNewUser(UserCreationModel newUser)
        {
            var user = new UserModel();

            user.Username = newUser.Username;
            user.Firstname = newUser.Firstname;
            user.Lastname = newUser.Lastname;

            return user;
        }

        public int DeleteUserById(int id)
        {
            return 1;
        }

        public UserModel GetUserById(int id)
        {
            if (id < 0)
                return null;

            var user = new UserModel();

            user.Id = (uint)id;
            user.Username = "Usernametest";
            user.Firstname = "testFirst";
            user.Lastname = "testLast";

            return user;
        }

        public int UpdateUserById(int id, UserUpdateModel updatedUser)
        {
            return 1;
        }
    }
}