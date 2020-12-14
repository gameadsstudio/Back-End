using System;
using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Contexts;
using GAME_ADS_STUDIO_API.Models.User;

namespace GAME_ADS_STUDIO_API.Repositories.User
{
    public class UserRepository : IUserRepository
    {

        private readonly ApiContext _context;

        public UserRepository(ApiContext context)
        {
            _context = context;
        }

        public int AddNewUser(UserModel User)
        {
            throw new NotImplementedException();
        }

        public int DeleteUser(UserModel User)
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
