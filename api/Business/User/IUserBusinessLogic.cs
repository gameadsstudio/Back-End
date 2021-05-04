using System;
using System.Security.Claims;
using api.Helpers;
using api.Models.User;

namespace api.Business.User
{
    public interface IUserBusinessLogic
    {
        Object GetUserById(string id, Claim currentUser);
        (int page, int pageSize, int maxPage, UserPublicDto[] users) GetUsers(PagingDto paging);
        UserPrivateDto AddNewUser(UserCreationDto newUser);
        UserPrivateDto UpdateUserById(string id, UserUpdateDto updatedUser, Claim currentUser);
        int DeleteUserById(string id, Claim currentUser);
        string Login(UserLoginDto userLoginDto);
        object GetSelf(Claim currentUser);
    }
}