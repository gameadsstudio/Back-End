using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using api.Contexts;
using api.Enums.User;
using api.Errors;
using api.Helpers;
using api.Models.User;
using api.Repositories.User;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace api.Business.User
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public UserBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new UserRepository(context);
            _mapper = mapper;
        }

        public IUserDto GetUserById(string id, ConnectedUser currentUser)
        {
            var user = GetUserModelById(id);

            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {id}");
            }

            if (user.Id == currentUser.Id)
            {
                return _mapper.Map(user, new UserPrivateDto());
            }

            return _mapper.Map(user, new UserPublicDto());
        }

        public UserModel GetUserModelById(string id)
        {
            var user = _repository.GetUserById(GuidHelper.StringToGuidConverter(id));

            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {id}");
            }

            return user;
        }

        public UserPrivateDto GetSelf(ConnectedUser currentUser)
        {
            var user = GetUserModelById(currentUser.Id.ToString());

            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {currentUser.Id}");
            }

            return _mapper.Map(user, new UserPrivateDto());
        }

        public (int page, int pageSize, int maxPage, IList<UserPublicDto> items) SearchUser(string search,
            PagingDto paging,
            ConnectedUser currentUser)
        {
            paging = PagingHelper.Check(paging);
            var (users, maxPage) = _repository.SearchUser((paging.Page - 1) * paging.PageSize, paging.PageSize, search);

            return (paging.Page, paging.PageSize, maxPage / paging.PageSize + 1,
                _mapper.Map(users, new List<UserPublicDto>()));
        }

        public (int page, int pageSize, int maxPage, IList<UserPublicDto> users) GetUsers(PagingDto paging,
            UserFiltersDto filters)
        {
            paging = PagingHelper.Check(paging);
            var (users, maxPage) = _repository.GetUsers((paging.Page - 1) * paging.PageSize, paging.PageSize, filters);
            return (paging.Page, paging.PageSize, maxPage, _mapper.Map(users, new List<UserPublicDto>()));
        }

        public UserPrivateDto AddNewUser(UserCreationDto newUser)
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
            var result = _repository.AddNewUser(user);
            return _mapper.Map(result, new UserPrivateDto());
        }

        public int DeleteUserById(string id, ConnectedUser currentUser)
        {
            var user = GetUserModelById(id);

            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {id}");
            }

            if (user.Id != currentUser.Id && currentUser.Role != UserRole.Admin)
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Cannot delete another user's account");
            }

            return _repository.DeleteUser(user);
        }

        public UserPrivateDto UpdateUserById(string id, UserUpdateDto updatedUser, ConnectedUser currentUser)
        {
            var user = GetUserModelById(id);

            if (user.Id != currentUser.Id)
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Cannot modify another user's account");
            }

            if (updatedUser.Username != null && _repository.GetUserByUsername(updatedUser.Username) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"User with username: {updatedUser.Username} already exists");
            }

            if (updatedUser.Email != null && _repository.GetUserByEmail(updatedUser.Email) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"User with email: {updatedUser.Email} already exists");
            }

            if (updatedUser.Password != null)
            {
                updatedUser.Password = HashHelper.HashPassword(user.Password);
            }

            if (updatedUser.Role != null && updatedUser.Role != UserRole.User && currentUser.Role != UserRole.Admin)
            {
                updatedUser.Role = UserRole.User;
            }

            user = _mapper.Map(updatedUser, user);
            var result = _repository.UpdateUser(user);
            return _mapper.Map(result, new UserPrivateDto());
        }

        public UserLoginResponseDto Login(UserLoginDto loginDto)
        {
            var user = EmailHelper.IsValidEmail(loginDto.Identifier)
                ? _repository.GetUserByEmail(loginDto.Identifier)
                : _repository.GetUserByUsername(loginDto.Identifier);

            if (user == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, "Couldn't find user with given identifier");
            }

            if (!HashHelper.ValidatePassword(loginDto.Password, user.Password))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Invalid password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("GAS_SECRET")!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new UserLoginResponseDto
            {
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}