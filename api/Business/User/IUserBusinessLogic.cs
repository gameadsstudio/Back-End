using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.User;

namespace api.Business.User
{
    public interface IUserBusinessLogic
    {
        IUserDto GetUserById(string id, ConnectedUser currentUser);
        UserModel GetUserModelById(string id);
        (int page, int pageSize, int maxPage, IList<UserPublicDto> users) GetUsers(PagingDto paging,
            UserFiltersDto filters);
        UserPrivateDto AddNewUser(UserCreationDto newUser);
        UserPrivateDto UpdateUserById(string id, UserUpdateDto updatedUser, ConnectedUser currentUser);
        int DeleteUserById(string id, ConnectedUser currentUser);
        UserLoginResponseDto Login(UserLoginDto userLoginDto);
        UserPrivateDto GetSelf(ConnectedUser currentUser);
        (int page, int pageSize, int maxPage, IList<UserPublicDto> items) SearchUser(string search, PagingDto paging,
            ConnectedUser currentUser);
    }
}