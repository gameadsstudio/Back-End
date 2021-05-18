using System;
using System.Collections.Generic;
using api.Models.User;

namespace api.Repositories.User
{
    public interface IUserRepository
    {
        UserModel AddNewUser(UserModel user);
        UserModel GetUserById(Guid id);
        UserModel GetUserByUsername(string username);
        UserModel GetUserByEmail(string email);
        List<UserModel> GetUsers(int offset, int limit);
        UserModel UpdateUser(UserModel updatedUser);
        int DeleteUser(UserModel user);
        int CountUsers();
        (List<UserModel> users, int count) SearchUser(int offset, int limit, string search);
    }
}
