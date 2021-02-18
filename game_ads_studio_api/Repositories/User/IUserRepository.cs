using game_ads_studio_api.Models.User;

namespace game_ads_studio_api.Repositories.User
{ 
    public interface IUserRepository
    {
        int AddNewUser(UserModel user);
        UserModel GetUserById(int id);
        int UpdateUser(UserModel updatedUser, UserModel targetUser);
        int DeleteUser(UserModel user);
    }
}
