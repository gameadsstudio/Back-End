using game_ads_studio_api.Models.User;

namespace game_ads_studio_api.Business.User
{
    internal interface IUserBusinessLogic
    {
        UserModel AddNewUser(UserCreationModel newUser);
        UserModel GetUserById(string id);
        UserModel UpdateUserById(string id, UserUpdateModel updatedUser);
        int DeleteUserById(string id);
    }
}
