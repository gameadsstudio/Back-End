using System;
using game_ads_studio_api.Configuration;
using game_ads_studio_api.Contexts;
using game_ads_studio_api.Models.User;
using game_ads_studio_api.Repositories.User;
using Microsoft.Extensions.Options;

namespace game_ads_studio_api.Business.User
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IUserRepository _repository;
        private readonly AppSettings _appSettings;

        public UserBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _repository = new UserRepository(context);
            _appSettings = appSettings.Value;
        }

        public UserModel AddNewUser(UserCreationModel newUser)
        {
            var user = new UserModel
            {
                Username = newUser.Username, Firstname = newUser.Firstname, Lastname = newUser.Lastname
            };

            throw new NotImplementedException();
        }

        public int DeleteUserById(string id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public UserModel UpdateUserById(string id, UserUpdateModel updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}