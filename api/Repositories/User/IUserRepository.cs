﻿using System;
using System.Collections.Generic;
using api.Models.User;

namespace api.Repositories.User
{
    public interface IUserRepository
    {
        UserModel AddNewUser(UserModel user);
        UserModel GetUserById(Guid id);
        UserModel GetUserByMSId(string id);
        UserModel GetUserByUsername(string username);
        UserModel GetUserByEmail(string email);
        UserModel GetUserByPasswordResetId(Guid passwordResetId);
        (IList<UserModel> users, int totalItemCount) GetUsers(int offset, int limit, UserFiltersDto filters);
        UserModel UpdateUser(UserModel updatedUser);
        int DeleteUser(UserModel user);
        int CountUsers();
        (IList<UserModel> users, int totalItemCount) SearchUser(int offset, int limit, string search);
        public UserModel GetUserByRefreshToken(UserRefreshDto refreshDto);
    }
}
