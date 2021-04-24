using System;
using api.Models.User;

namespace api.Repositories.User
{ 
    public interface IUserRepository
    {
        UserModel AddNewUser(UserModel user);
        UserModel GetUserById(Guid id);
        UserPublicModel[] GetUsers(int offset, int limit);
        UserModel UpdateUser(UserModel updatedUser, UserModel targetUser);
        int DeleteUser(UserModel user);
    }
}
