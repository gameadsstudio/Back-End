using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.User;

namespace api.Business.User
{
    public interface IUserBusinessLogic
    {
        object GetUserById(string id, ConnectedUser currentUser);
        UserModel GetUserModelById(Guid id);
        UserModel GetUserModelByEmail(string email);
        (int page, int pageSize, int totalItemCount, IList<UserPublicDto> users) GetUsers(PagingDto paging, UserFiltersDto filters, ConnectedUser user);
        UserPrivateDto AddNewUser(UserCreationDto newUser);
        UserPrivateDto UpdateUserById(string id, UserUpdateDto updatedUser, ConnectedUser currentUser);
        int DeleteUserById(string id, ConnectedUser currentUser);
        UserLoginResponseDto Login(UserLoginDto userLoginDto);
        UserPrivateDto GetSelf(ConnectedUser currentUser);
        void ConfirmEmail(ConnectedUser currentUser, Guid id);
        (int page, int pageSize, int totalItemCount, IList<UserPublicDto> items) SearchUser(string search, PagingDto paging, ConnectedUser currentUser);
        UserModel CreatePasswordResetId(UserModel user);
        void ResetPassword(UserResetDto resetDto);
    }
}
