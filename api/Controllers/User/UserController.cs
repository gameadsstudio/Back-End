using api.Business.User;
using api.Configuration;
using api.Contexts;
using api.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers.User
{
    [Route("/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessLogic _business;

        public UserController(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _business = new UserBusinessLogic(context, appSettings);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var userGet = _business.GetUserById(id);

            if (userGet != null)
                return Ok(userGet);
            return NotFound("User not found.");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] UserCreationModel newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.AddNewUser(newUser);

            if (success != null)
                return Created("User", success);
            return Conflict(new { message = "Couldn't create User" });
        }

        [AllowAnonymous]
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] UserUpdateModel newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateUserById(id, newUser));
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromForm] UserUpdateModel newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateUserById(id, newUser));
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var success = _business.DeleteUserById(id);

            return success switch
            {
                1 => Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }

    }
}
