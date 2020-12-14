using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Models.User;

namespace GAME_ADS_STUDIO_API.Repositories.User
{ 
    public interface IUserRepository
    {
        int AddNewUser(UserModel User);
        UserModel GetUserById(int id);
        int UpdateUser(UserModel updatedUser, UserModel targetUser);
        int DeleteUser(UserModel User);
    }
}
