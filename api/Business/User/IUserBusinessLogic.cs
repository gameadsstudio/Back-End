using System;
using api.Helpers;
using api.Models.User;

namespace api.Business.User
{
    internal interface IUserBusinessLogic
    {
        UserModel GetUserById(string id);
        (int page, int pageSize, int maxPage, UserPublicModel[] users) GetUsers(PagingDto paging);
        UserModel AddNewUser(UserCreationModel newUser);
        UserModel UpdateUserById(string id, UserUpdateModel updatedUser);
        int DeleteUserById(string id);
        string Login(UserLoginModel userLoginModel);
    }
}
