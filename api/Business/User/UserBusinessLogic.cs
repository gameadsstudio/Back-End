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
            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {id}");
            }

            return user;
        }

        public (int, int, int, UserPublicModel[]) GetUsers(PagingDto paging)
        {
            var maxPage = _repository.CountUsers() / paging.pageSize + 1;
            var users = _repository.GetUsers((paging.page - 1) * paging.pageSize, paging.pageSize);
            return (paging.page, paging.pageSize, maxPage, users);
        }

        public UserModel AddNewUser(UserCreationModel newUser)
        {
            var user = _mapper.Map(newUser, new UserModel());

            if (_repository.GetUserByUsername(user.Username) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"User with username: {user.Username} already exists");
            }

            if (_repository.GetUserByEmail(user.Email) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"User with email: {user.Email} already exists");
            }

            user.Password = HashHelper.HashPassword(user.Password);
            return _repository.AddNewUser(user);
        }

        public int DeleteUserById(string id)
        {
            var user = _repository.GetUserById(Guid.Parse(id));
            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {id}");
            }

            return _repository.DeleteUser(user);
        }

        public UserModel UpdateUserById(string id, UserUpdateModel updatedUser)
        {
            var user = _repository.GetUserById(Guid.Parse(id));
            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {id}");
            }

            throw new NotImplementedException();
        }

        public string Login(UserLoginModel loginModel)
        {
            UserModel user = null;

            if (EmailHelper.IsValidEmail(loginModel.Identifier))
            {
                user = _repository.GetUserByEmail(loginModel.Identifier);
            }
            else
            {
                user = _repository.GetUserByUsername(loginModel.Identifier);
            }

            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, "Couldn't find user with given identifier");
            }

            if (!HashHelper.ValidatePassword(loginModel.Password, user.Password))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Invalid password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("GAS_SECRET")!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =
                    new ClaimsIdentity(new Claim[] {new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())}),
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