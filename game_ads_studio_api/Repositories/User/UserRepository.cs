using System;
using game_ads_studio_api.Contexts;
using game_ads_studio_api.Models.User;

namespace game_ads_studio_api.Repositories.User
{
    public class UserRepository : IUserRepository
    {

        private readonly ApiContext _context;

        public UserRepository(ApiContext context)
        {
            _context = context;
        }

        public int AddNewUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        public int DeleteUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateUser(UserModel updatedUser, UserModel targetUser)
        {
            throw new NotImplementedException();
        }

    }
}
