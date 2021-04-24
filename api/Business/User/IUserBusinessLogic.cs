using api.Helpers;
using api.Models.User;

namespace api.Business.User
{
    internal interface IUserBusinessLogic
    {
        UserModel GetUserById(string id);
        UserPublicModel[] GetUsers(PagingDto paging);
        UserModel AddNewUser(UserCreationModel newUser);
        UserModel UpdateUserById(string id, UserUpdateModel updatedUser);
        int DeleteUserById(string id);
    }
}
