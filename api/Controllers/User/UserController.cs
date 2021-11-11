using System;
using System.Net.Mail;
using System.Net;
using api.Business.User;
using api.Business.Mail;
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

        private readonly IMailBusinessLogic _businessMail;

        public UserController(
            IUserBusinessLogic userBusinessLogic,
            IMailBusinessLogic mailBusinessLogic
        )
        {
            _business = userBusinessLogic;
            _businessMail = mailBusinessLogic;
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
            UserPrivateDto user = _business.AddNewUser(newUser);
            UserModel userData = _business.GetUserModelById(user.Id);
            string callbackUrl = Environment.GetEnvironmentVariable(
                "GAS_MAIL_CALLBACK_URL"
            );

            callbackUrl.TrimEnd('/');
            _businessMail.send(
                userData.Email,
                "Confirm your email address",
                "You can confirm your email address with this URL: "
                + $"{callbackUrl}/{userData.EmailValidatedId}"
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
        [HttpPost("forgot")]
        public IActionResult Forgot([FromForm] UserForgotDto forgotDto)
        {
            UserModel user = null;
            string callbackUrl = Environment.GetEnvironmentVariable(
                "GAS_MAIL_CALLBACK_FORGOT_PASSWORD"
            );

            callbackUrl.TrimEnd('/');
            try {
                user = _business.CreatePasswordResetId(
                    _business.GetUserModelByEmail(forgotDto.Email)
                );
                _businessMail.send(
                    user.Email,
                    "Reset your password",
                    "You can reset your password here: "
                    + $"{callbackUrl}/{user.PasswordResetId}"
                );
            }
            catch {
                Console.WriteLine("Error");
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public IActionResult Reset([FromForm] UserResetDto resetDto)
        {
            _business.ResetPassword(resetDto);
            return Ok();
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
