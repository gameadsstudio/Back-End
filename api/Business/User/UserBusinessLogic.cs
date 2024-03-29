﻿using System;
using System.Data;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Contexts;
using api.Enums.User;
using api.Errors;
using api.Helpers;
using api.Models.User;
using api.Business.Mail;
using api.Repositories.User;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace api.Business.User
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly UriHelper _uriHelper;
        private readonly IMailBusinessLogic _businessMail;

        public UserBusinessLogic(ApiContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMailBusinessLogic mailBusinessLogic)
        {
            _repository = new UserRepository(context);
            _mapper = mapper;
            _uriHelper = new UriHelper(httpContextAccessor);
            _businessMail = mailBusinessLogic;
        }

        // Todo : Find a better return type
        public object GetUserById(string id, ConnectedUser currentUser)
        {
            var user = GetUserModelById(new Guid(id));

            if (user == null)
            {
                throw new UserNotFoundError($"Couldn't find user with Id: {id}");
            }

            if (user.Id == currentUser.Id)
            {
                return _mapper.Map(user, new UserPrivateDto());
            }

            return _mapper.Map(user, new UserPublicDto());
        }

        public UserModel GetUserModelById(Guid id)
        {
            var user = _repository.GetUserById(id);

            if (user == null)
            {
                throw new UserNotFoundError($"Couldn't find user with Id: {id}");
            }

            return user;
        }

        public UserModel GetUserModelByEmail(string email)
        {
            return _repository.GetUserByEmail(email);
        }

        public UserPrivateDto GetSelf(ConnectedUser currentUser)
        {
            var user = GetUserModelById(currentUser.Id);

            if (user == null)
            {
                throw new UserNotFoundError($"Couldn't find user with Id: {currentUser.Id}");
            }

            return _mapper.Map(user, new UserPrivateDto());
        }

        public (int page, int pageSize, int totalItemCount, IList<UserPublicDto> items) SearchUser(string search,
            PagingDto paging, ConnectedUser currentUser)
        {
            paging = PagingHelper.Check(paging);
            var (users, totalItemCount) =
                _repository.SearchUser((paging.Page - 1) * paging.PageSize, paging.PageSize, search);

            return (paging.Page, paging.PageSize, totalItemCount, _mapper.Map(users, new List<UserPublicDto>()));
        }

        public (int page, int pageSize, int totalItemCount, IList<UserPublicDto> users) GetUsers(PagingDto paging,
            UserFiltersDto filters, ConnectedUser user)
        {
            paging = PagingHelper.Check(paging);
            var currentUser = GetUserModelById(user.Id);

            if (filters.OrganizationId != Guid.Empty &&
                currentUser.Organizations.All(p => p.Id != filters.OrganizationId))
            {
                throw new UserInvalidRightsError("Cannot get users from an organization which you are not a part of");
            }

            var (users, totalItemCount) =
                _repository.GetUsers((paging.Page - 1) * paging.PageSize, paging.PageSize, filters);
            return (paging.Page, paging.PageSize, totalItemCount, _mapper.Map(users, new List<UserPublicDto>()));
        }

        public UserPrivateDto AddNewUser(UserCreationDto newUser)
        {
            var user = _mapper.Map(newUser, new UserModel());
            var callbackUrl = Environment.GetEnvironmentVariable(
                "GAS_MAIL_CALLBACK_URL"
            );

            if (_repository.GetUserByUsername(user.Username) != null)
            {
                throw new UsernameAlreadyExistError($"User with username: {user.Username} already exists");
            }
            if (_repository.GetUserByEmail(user.Email) != null)
            {
                throw new UserEmailAlreadyExistError($"User with email: {user.Email} already exists");
            }
            callbackUrl.TrimEnd('/');
            user.EmailValidated = false;
            user.EmailValidatedId = Guid.NewGuid();
            user.Password = HashHelper.HashPassword(user.Password);
            user.Role = UserRole.User;
            user.ProfilePictureUrl = new Uri("about:blank");
            var result = _repository.AddNewUser(user);
            result = AddOrReplaceProfilePicture(result, newUser.ProfilePicture);
            _businessMail.send(
                result.Email,
                "Confirm your email address",
                "You can confirm your email address with this URL: "
                + $"{callbackUrl}/{result.EmailValidatedId}"
            );
            return _mapper.Map(result, new UserPrivateDto());
        }

        private UserModel AddOrReplaceProfilePicture(UserModel user, IFormFile picture)
        {
            if (picture == null)
            {
                return user;
            }

            var assetsDir = $"/assets/users/{user.Id.ToString()}";

            // Create user dir if not exists
            if (!Directory.Exists(assetsDir))
            {
                Directory.CreateDirectory(assetsDir);
            }

            // TODO check for file format before saving
            // Saving texture
            using (var fileStream = new FileStream($"{assetsDir}/profilePicture{Path.GetExtension(picture.FileName)}",
                FileMode.Create))
            {
                picture.CopyTo(fileStream);
                user.ProfilePictureUrl = _uriHelper.UriBuilder(fileStream.Name);
            }

            return _repository.UpdateUser(user);
        }

        public int DeleteUserById(string id, ConnectedUser currentUser)
        {
            var user = GetUserModelById(new Guid(id));

            if (user == null)
            {
                throw new UserNotFoundError($"Couldn't find user with Id: {id}");
            }

            if (user.Id != currentUser.Id && currentUser.Role != UserRole.Admin)
            {
                throw new UserInvalidRightsError("Cannot delete another user's account");
            }

            var assetsDir = $"/assets/users/{user.Id.ToString()}";

            // Delete assets dir if exists
            if (Directory.Exists(assetsDir))
            {
                Directory.Delete(assetsDir, true);
            }

            return _repository.DeleteUser(user);
        }

        public UserPrivateDto UpdateUserById(string id, UserUpdateDto updatedUser, ConnectedUser currentUser)
        {
            var user = GetUserModelById(new Guid(id));

            if (user.Id != currentUser.Id)
            {
                throw new UserInvalidRightsError("Cannot modify another user's account");
            }

            if (updatedUser.Username != null && _repository.GetUserByUsername(updatedUser.Username) != null)
            {
                throw new UsernameAlreadyExistError($"User with username: {updatedUser.Username} already exists");
            }

            if (updatedUser.Email != null && _repository.GetUserByEmail(updatedUser.Email) != null)
            {
                throw new UserEmailAlreadyExistError("User with email: {updatedUser.Email} already exists");
            }

            if (updatedUser.Password != null)
            {
                updatedUser.Password = HashHelper.HashPassword(updatedUser.Password);
            }

            if (updatedUser.Role != null && updatedUser.Role != UserRole.User && currentUser.Role != UserRole.Admin)
            {
                updatedUser.Role = UserRole.User;
            }

            user = _mapper.Map(updatedUser, user);
            var result = _repository.UpdateUser(user);

            result = AddOrReplaceProfilePicture(result, updatedUser.ProfilePicture);

            return _mapper.Map(result, new UserPrivateDto());
        }

        public UserLoginResponseDto Login(UserLoginDto loginDto)
        {
            var user = EmailHelper.IsValidEmail(loginDto.Identifier)
                ? _repository.GetUserByEmail(loginDto.Identifier)
                : _repository.GetUserByUsername(loginDto.Identifier);

            if (user == null)
            {
                throw new UserNotFoundError("Couldn't find user with given identifier");
            }

            if (!HashHelper.ValidatePassword(loginDto.Password, user.Password))
            {
                throw new InvalidCredentialsError("Invalid password");
            }

            return GenerateLoginResponseDto(user);
        }

        public UserLoginResponseDto Refresh(UserRefreshDto refreshDto)
        {
            var user = _repository.GetUserByRefreshToken(refreshDto);

            if (user == null)
            {
                throw new UserBadRequestError();
            }
            
            return GenerateLoginResponseDto(user);
        }

        private UserLoginResponseDto GenerateLoginResponseDto(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("GAS_SECRET")!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(
                            "email_validated",
                            user.EmailValidated.ToString()
                        )
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var lenght = 64;
            var res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (lenght -- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            var refreshToken = res.ToString();

            user.RefreshToken = refreshToken;
            _repository.UpdateUser(user);

            return new UserLoginResponseDto {Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken};
        }

        public void ConfirmEmail(ConnectedUser currentUser, Guid id)
        {
            var user = _repository.GetUserById(currentUser.Id);

            if (user.EmailValidatedId == Guid.Empty)
            {
                throw new AccountValidationError("Email already validated");
            }
            else if (user.EmailValidatedId != id)
            {
                throw new AccountValidationError("Invalid confirmation code");
            }
            user.EmailValidated = true;
            user.EmailValidatedId = Guid.Empty;
            _repository.UpdateUser(user);
        }

        public UserModel CreatePasswordResetId(UserModel user)
        {
            user.PasswordResetId = Guid.NewGuid();
            return _repository.UpdateUser(user);
        }

        public void ResetPassword(UserResetDto resetDto)
        {
            UserModel user = null;

            if (resetDto.PasswordResetId == Guid.Empty)
            {
                throw new ResetPasswordError("Guid cannot be null");
            }
            user = _repository.GetUserByPasswordResetId(
                resetDto.PasswordResetId
            );
            user.Password = HashHelper.HashPassword(resetDto.Password);
            user.PasswordResetId = Guid.Empty;
            _repository.UpdateUser(user);
        }

        public void ForgotPassword(UserForgotDto forgotDto)
        {
            UserModel user = null;
            string callbackUrl = Environment.GetEnvironmentVariable("GAS_MAIL_CALLBACK_FORGOT_PASSWORD");

            callbackUrl.TrimEnd('/');
            try {
                user = this.CreatePasswordResetId(
                    this.GetUserModelByEmail(forgotDto.Email)
                );
                _businessMail.send(
                    user.Email, "Reset your password",
                    "You can reset your password here: "
                    + $"{callbackUrl}/{user.PasswordResetId}"
                );
            }
            catch (DataException) {
                // NTD
            }
        }
    }
}
