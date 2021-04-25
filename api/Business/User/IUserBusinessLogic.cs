using System.Security.Claims;
using api.Helpers;
using api.Models.User;

namespace api.Business.User
{
    public interface IUserBusinessLogic
    {
        IUserModel GetUserById(string id, Claim currentUser);
        (int page, int pageSize, int maxPage, UserPublicModel[] users) GetUsers(PagingDto paging);
        UserPrivateModel AddNewUser(UserCreationModel newUser);
        UserPrivateModel UpdateUserById(string id, UserUpdateModel updatedUser, Claim currentUser);
        int DeleteUserById(string id, Claim currentUser);
        string Login(UserLoginModel userLoginModel);
    }
}