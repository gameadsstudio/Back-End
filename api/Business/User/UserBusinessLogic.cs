using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using api.Configuration;
using api.Contexts;
using api.Errors;
using api.Helpers;
using api.Models.User;
using api.Repositories.User;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Business.User
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public UserBusinessLogic(ApiContext context,
            IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            _repository = new UserRepository(context);
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public object GetUserById(string id,
            Claim currentUser)
        {
            var user = _repository.GetUserById(Guid.Parse(id));
            if (user == null)
                throw new ApiError(HttpStatusCode.NotFound,
                    $"Couldn't find user with Id: {id}");

            if (user.Id.ToString() == currentUser.Value)
                return _mapper.Map(user,
                    new UserPrivateModel());

            return _mapper.Map(user,
                new UserPublicModel());
        }

        public (int, int, int, UserPublicModel[]) GetUsers(PagingDto paging)
        {
            var maxPage = _repository.CountUsers() / paging.pageSize + 1;
            var users = _repository.GetUsers((paging.page - 1) * paging.pageSize,
                paging.pageSize);
            return (paging.page, paging.pageSize, maxPage, users);
        }

        public UserPrivateModel AddNewUser(UserCreationModel newUser)
        {
            var user = _mapper.Map(newUser,
                new UserModel());
            if (_repository.GetUserByUsername(user.Username) != null)
                throw new ApiError(HttpStatusCode.Conflict,
                    $"User with username: {user.Username} already exists");

            if (_repository.GetUserByEmail(user.Email) != null)
                throw new ApiError(HttpStatusCode.Conflict,
                    $"User with email: {user.Email} already exists");

            user.Password = HashHelper.HashPassword(user.Password);
            var result = _repository.AddNewUser(user);
            return _mapper.Map(result,
                new UserPrivateModel());
        }

        public int DeleteUserById(string id,
            Claim currentUser)
        {
            var user = _repository.GetUserById(Guid.Parse(id));
            if (user == null)
                throw new ApiError(HttpStatusCode.NotFound,
                    $"Couldn't find user with Id: {id}");

            if (user.Id.ToString() != currentUser.Value)
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot delete another user's account");

            return _repository.DeleteUser(user);
        }

        public UserPrivateModel UpdateUserById(string id,
            UserUpdateModel updatedUser,
            Claim currentUser)
        {
            var user = _repository.GetUserById(Guid.Parse(id));
            if (user == null)
                throw new ApiError(HttpStatusCode.NotFound,
                    $"Couldn't find user with Id: {id}");

            if (user.Id.ToString() != currentUser.Value)
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot modify another user's account");

            if (updatedUser.Username != null && _repository.GetUserByUsername(updatedUser.Username) != null)
                throw new ApiError(HttpStatusCode.Conflict,
                    $"User with username: {updatedUser.Username} already exists");

            if (updatedUser.Email != null && _repository.GetUserByEmail(updatedUser.Email) != null)
                throw new ApiError(HttpStatusCode.Conflict,
                    $"User with email: {updatedUser.Email} already exists");

            if (updatedUser.Password != null)
                updatedUser.Password = HashHelper.HashPassword(user.Password);

            user = _mapper.Map(updatedUser,
                user);
            var result = _repository.UpdateUser(user);
            return _mapper.Map(result,
                new UserPrivateModel());
        }

        public string Login(UserLoginModel loginModel)
        {
            UserModel user = null;
            if (EmailHelper.IsValidEmail(loginModel.Identifier))
                user = _repository.GetUserByEmail(loginModel.Identifier);
            else
                user = _repository.GetUserByUsername(loginModel.Identifier);

            if (user == null)
                throw new ApiError(HttpStatusCode.NotFound,
                    "Couldn't find user with given identifier");

            if (!HashHelper.ValidatePassword(loginModel.Password,
                user.Password))
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Invalid password");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("GAS_SECRET")!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new(ClaimTypes.NameIdentifier,
                        user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            ;
        }
    }
}