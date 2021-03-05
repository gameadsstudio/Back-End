using api.Models.User;

namespace api.Repositories.User
{ 
    public interface IUserRepository
    {
        int AddNewUser(UserModel user);
        UserModel GetUserById(int id);
        int UpdateUser(UserModel updatedUser, UserModel targetUser);
        int DeleteUser(UserModel user);
    }
}
