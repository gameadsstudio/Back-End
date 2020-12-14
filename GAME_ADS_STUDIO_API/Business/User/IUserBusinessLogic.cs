using GAME_ADS_STUDIO_API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Business.User
{
    interface IUserBusinessLogic
    {
        UserModel AddNewUser(UserCreationModel newUser);
        UserModel GetUserById(int id);
        int UpdateUserById(int id, UserUpdateModel updatedUser);
        int DeleteUserById(int id);
    }
}
