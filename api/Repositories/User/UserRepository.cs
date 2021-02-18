using System;
using api.Contexts;
using api.Models.User;

namespace api.Repositories.User
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
