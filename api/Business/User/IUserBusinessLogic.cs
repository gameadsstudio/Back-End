using System.Collections.Generic;
using System.Security.Claims;
using api.Helpers;
using api.Models.User;

namespace api.Business.User
{
    public interface IUserBusinessLogic
    {
        IUserDto GetUserById(string id, Claim currentUser);
        UserModel GetUserModelById(string id);
        (int page, int pageSize, int maxPage, List<UserPublicDto> users) GetUsers(PagingDto paging);
        UserPrivateDto AddNewUser(UserCreationDto newUser);
        UserPrivateDto UpdateUserById(string id, UserUpdateDto updatedUser, Claim currentUser);
        int DeleteUserById(string id, Claim currentUser);
        UserLoginResponseDto Login(UserLoginDto userLoginDto);
        UserPrivateDto GetSelf(Claim currentUser);
        (int page, int pageSize, int maxPage, IEnumerable<IUserDto> items) SearchUser(string search, PagingDto paging,
            Claim currentUser, bool strict);
    }
}