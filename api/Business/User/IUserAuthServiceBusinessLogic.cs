using api.Models.User;

namespace api.Business.User
{
    public interface IUserAuthServiceBusinessLogic
    {
        UserLoginResponseDto Login(UserLoginServiceDto loginServiceDto);
    }
}
