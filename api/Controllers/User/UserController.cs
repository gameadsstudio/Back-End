using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using api.Business.User;
using api.Helpers;
using api.Models.Common;
using api.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.User
{
    [Route("/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessLogic _business;

        private readonly IUserAuthServiceBusinessLogic _businessAuthService;

        public UserController(IUserBusinessLogic userBusinessLogic, IUserAuthServiceBusinessLogic userAuthServiceBusinessLogic)
        {
            _business = userBusinessLogic;
            _businessAuthService = userAuthServiceBusinessLogic;
        }

        [HttpGet("self")]
        public ActionResult<GetDto<UserPrivateDto>> GetSelf()
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<UserPrivateDto>(_business.GetSelf(currentUser)));
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<object>> GetUser(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);
            return Ok(new GetDto<object>(_business.GetUserById(id, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<UserPublicDto>> GetAll([FromQuery] PagingDto paging, [FromQuery] UserFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<UserPublicDto>(_business.GetUsers(paging, filters, currentUser)));
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<GetDto<UserPrivateDto>> Post([FromForm] UserCreationDto newUser)
        {
            var user = _business.AddNewUser(newUser);
            var client = new SmtpClient(
                Environment.GetEnvironmentVariable(
                    "GAS_MAIL_HOST"
                ) ?? "smtp.mailtrap.io",
                Int32.Parse(
                    Environment.GetEnvironmentVariable(
                        "GAS_MAIL_PORT"
                    ) ?? "2525"
                )
            )
            {
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable(
                        "GAS_MAIL_USERNAME"
                    ) ?? "feae1f21794992",
                    Environment.GetEnvironmentVariable(
                        "GAS_MAIL_PASSWORD"
                    ) ?? "42f8ea37006689"
                ),
                EnableSsl = bool.Parse(
                    Environment.GetEnvironmentVariable("GAS_MAIL_SSL") ?? "true"
                )
            };
            var userData = _business.GetUserModelById(user.Id.ToString());

            client.Send(
                Environment.GetEnvironmentVariable(
                    "GAS_MAIL_ADR_NO_REPLY"
                ) ?? "no-reply@gameadsstudio.com",
                userData.Email,
                "Confirm your email address",
                $"You can confirm your email address with this URL: https://example.com/email/{userData.EmailValidatedId}"
            );

            return Created("User", new GetDto<UserPrivateDto>(user));
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<UserPrivateDto>> Patch(string id, [FromForm] UserUpdateDto newUser)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<UserPrivateDto>(_business.UpdateUserById(id, newUser, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.DeleteUserById(id, currentUser);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<GetDto<UserLoginResponseDto>> Login([FromForm] UserLoginDto loginDto)
        {
            return Ok(new GetDto<UserLoginResponseDto>(_business.Login(loginDto)));
        }

        [AllowAnonymous]
        [HttpPost("login-service")]
        public ActionResult<GetDto<UserLoginResponseDto>> LoginFromService([FromForm] UserLoginServiceDto loginServiceDto)
        {
            return Ok(new GetDto<UserLoginResponseDto>(_businessAuthService.Login(loginServiceDto)));
        }

        [HttpGet("search/{search}")]
        public ActionResult<GetAllDto<UserPublicDto>> SearchUser(string search, [FromQuery] PagingDto paging)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<UserPublicDto>(_business.SearchUser(search, paging, currentUser)));
        }

        [HttpPost("email/{id}")]
        public IActionResult EmailValidation(Guid id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.ConfirmEmail(currentUser, id);
            return Ok();
        }
    }
}
