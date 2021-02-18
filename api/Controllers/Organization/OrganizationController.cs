using api.Business.Organization;
using api.Configuration;
using api.Contexts;
using api.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers.Organization
{
    [Route("/organizations")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationBusinessLogic _business;

        public OrganizationController(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _business = new OrganizationBusinessLogic(context, appSettings);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var organizationGet = _business.GetOrganizationById(id);

            if (organizationGet != null)
                return Ok(organizationGet);
            return NotFound("Organization not found.");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] OrganizationCreationModel newOrganization)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.AddNewOrganization(newOrganization);

            if (success != null)
                return Created("Organizations", success);
            return Conflict(new { message = "Couldn't create organization" });
        }

        [AllowAnonymous]
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] OrganizationUpdateModel newOrganization)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateOrganizationById(id, newOrganization));
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromForm] OrganizationUpdateModel newOrganization)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateOrganizationById(id, newOrganization));
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(_business.DeleteOrganizationById(id));
        }

        [AllowAnonymous]
        [HttpPost("{id}/users/{userId}")]
        public IActionResult AddUserToOrganization(string id, string userId)
        {
            var success = _business.AddUserToOrganization(id, userId);

            return success switch
            {
                1 => Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpGet("{id}/users")]
        public IActionResult GetOrganizationUsers(string id)
        {
            var users = _business.GetOrganizationUsers(id);

            if (users != null)
                return Ok(users);
            return NotFound("Organization not found.");
        }

        [AllowAnonymous]
        [HttpDelete("{id}/users/{userId}")]
        public IActionResult DeleteUserFromOrganization(string id, string userId)
        {
            var success = _business.DeleteUserFromOrganization(id, userId);

            return success switch
            {
                1 => Ok("Deleted user from organization successfully"),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
