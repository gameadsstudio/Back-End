using System;
using System.Text;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using api.Contexts;
using api.Enums.Auth;
using api.Errors;
using api.Models.User;
using api.Repositories.User;

namespace api.Business.User
{
    public class UserAuthServiceBusinessLogic : IUserAuthServiceBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public UserAuthServiceBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new UserRepository(context);
            _mapper = mapper;
        }

        public UserLoginResponseDto Login(UserLoginServiceDto loginServiceDto)
        {
            if (!Enum.IsDefined<Providers>(loginServiceDto.Provider)) {
                throw new AuthenticationServiceNotFound();
            }
            else if (loginServiceDto.Provider == Providers.Microsoft) {
                return this.Microsoft(loginServiceDto.Token);
            }
            return null;
        }

        private void AddNewUser(UserModel user)
        {
            if (_repository.GetUserByUsername(user.Username) != null)
            {
                throw new UsernameAlreadyExistError($"User with username: {user.Username} already exists");
            }
            if (_repository.GetUserByEmail(user.Email) != null)
            {
                throw new UserEmailAlreadyExistError($"User with email: {user.Email} already exists");
            }
            _repository.AddNewUser(user);
        }

        private UserLoginResponseDto Microsoft(string token)
        {
            HttpResponseMessage httpResponseMessage = null;
            JsonElement json;
            UserModel user = null;

            using (var httpClient = new HttpClient())
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/me"))
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        token
                    );
                    httpResponseMessage = httpClient.SendAsync(
                        requestMessage
                    ).Result;
                }
            }
            if (!httpResponseMessage.IsSuccessStatusCode) {
                throw new BadTokenError();
            }
            json = JsonDocument.Parse(
                httpResponseMessage.Content.ReadAsStringAsync().Result
            ).RootElement;
            user = _repository.GetUserByMSId(json.GetProperty("id").ToString());
            if (user == null) {
                user = new UserModel();
                user.MicrosoftId = json.GetProperty("id").ToString();
                user.Username = json.GetProperty("displayName").ToString();
                user.LastName = json.GetProperty("surname").ToString();
                user.FirstName = json.GetProperty("givenName").ToString();
                user.Email = json.GetProperty("userPrincipalName").ToString();
                user.Role = Enums.User.UserRole.User;
                this.AddNewUser(user);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(
                Environment.GetEnvironmentVariable("GAS_SECRET")!
            );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            return new UserLoginResponseDto
            {
                Token = tokenHandler.WriteToken(
                    tokenHandler.CreateToken(tokenDescriptor)
                )
            };
        }
    }
}
