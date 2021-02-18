using api.Models.User;

namespace api.Business.User
{
    internal interface IUserBusinessLogic
    {
        UserModel AddNewUser(UserCreationModel newUser);
        UserModel GetUserById(string id);
        UserModel UpdateUserById(string id, UserUpdateModel updatedUser);
        int DeleteUserById(string id);
    }
}
