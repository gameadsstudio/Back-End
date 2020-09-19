using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Models.User;

namespace GAME_ADS_STUDIO_API.Data.User
{ 
    public interface IUserRepository
    {
        IEnumerable<UserModel> GetUsers();
        UserModel GetUserById(int id);
    }
}
