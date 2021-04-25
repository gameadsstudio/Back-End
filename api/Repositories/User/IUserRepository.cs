using System;
using api.Models.User;

namespace api.Repositories.User
{ 
    public interface IUserRepository
    {
        UserModel AddNewUser(UserModel user);
        UserModel GetUserById(Guid id);
        UserModel GetUserByUsername(string username);
        UserModel GetUserByEmail(string email);
        UserPublicModel[] GetUsers(int offset, int limit);
        UserModel UpdateUser(UserModel updatedUser);
        int DeleteUser(UserModel user);
        int CountUsers();
    }
}
