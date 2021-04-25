using System.Linq;
using System.Security.Claims;
using api.Business.User;
using api.Configuration;
using api.Contexts;
using api.Helpers;
using api.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers.User
{
    [Authorize]
    [Route("/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessLogic _business;

        public UserController(ApiContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _business = new UserBusinessLogic(context, appSettings, mapper);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var user = _business.GetUserById(id, currentUser);
            return Ok(new {status = 200, user});
        }
        
        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging)
        {
            var result = _business.GetUsers(PagingHelper.Check(paging));

            return Ok(new
            {
                status = 200,
                page = result.Item1,
                pagesize = result.Item2,
                maxPage = result.Item3,
                users = result.Item4
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] UserCreationModel newUser)
        {
            var user = _business.AddNewUser(newUser);

            return Created("User", new {status = 201, user});
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] UserUpdateModel newUser)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(_business.UpdateUserById(id, newUser, currentUser));
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
        public IActionResult Login([FromForm] UserLoginModel loginModel)
        {
            var token = _business.Login(loginModel);

            return Ok(new {status = 200, token});
        }
    }
}