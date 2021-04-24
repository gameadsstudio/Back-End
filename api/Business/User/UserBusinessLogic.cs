using System;
using api.Configuration;
using api.Contexts;
using api.Helpers;
using api.Models.User;
using api.Repositories.User;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace api.Business.User
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _repository = new UserRepository(context);
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public UserModel GetUserById(string id)
        {
            var user = _repository.GetUserById(Guid.Parse(id));
            return user;
        }
        
        public UserPublicModel[] GetUsers(PagingDto paging)
        {
            var users = _repository.GetUsers((paging.page - 1) * paging.pageSize, paging.pageSize);
            return users;
        }
        public UserModel AddNewUser(UserCreationModel newUser)
        {
            var user = _mapper.Map(newUser, new UserModel());
            user.Password = HashHelper.HashPassword(user.Password);
            return _repository.AddNewUser(user);
        }

        public int DeleteUserById(string id)
        {
            var user = _repository.GetUserById(Guid.Parse(id));
            return _repository.DeleteUser(user);
        }

        public UserModel UpdateUserById(string id, UserUpdateModel updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}