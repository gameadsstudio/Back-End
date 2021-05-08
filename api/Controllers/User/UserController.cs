using System.Linq;
using System.Security.Claims;
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

        public UserController(IUserBusinessLogic userBusinessLogic)
        {
            _business = userBusinessLogic;
        }

        [HttpGet("self")]
        public IActionResult GetSelf()
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var user = _business.GetSelf(currentUser);
            return Ok(new {status = 200, user});
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<IUserDto>> GetUser(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var user = _business.GetUserById(id, currentUser);
            return Ok(new GetDto<IUserDto>()
            {
                Data = user,
            });
        }

        [HttpGet]
        public ActionResult<GetAllDto<UserPublicDto>> GetAll([FromQuery] PagingDto paging)
        {
            var (page, pageSize, maxPage, users) = _business.GetUsers(paging);
            
            return Ok(new GetAllDto<UserPublicDto>()
            {
                Data =
                {
                    PageIndex = page,
                    ItemsPerPage = pageSize,
                    TotalPages = maxPage,
                    CurrentItemCount = users.Count,
                    Items = users
                }
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<GetDto<UserPrivateDto>> Post([FromForm] UserCreationDto newUser)
        {
            var user = _business.AddNewUser(newUser);

            return Created("User", new GetDto<UserPrivateDto>()
            {
                Data = user
            });
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<UserPrivateDto>> Patch(string id, [FromForm] UserUpdateDto newUser)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(new GetDto<UserPrivateDto>()
            {
                Data = _business.UpdateUserById(id, newUser, currentUser)
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            _business.DeleteUserById(id, currentUser);

            return Ok(new {status = 200,});
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<GetDto<UserLoginResponseDto>> Login([FromForm] UserLoginDto loginDto)
        {
            var response = _business.Login(loginDto);

            return Ok(new GetDto<UserLoginResponseDto>()
            {
                Data = response,
            });
        }
    }
}