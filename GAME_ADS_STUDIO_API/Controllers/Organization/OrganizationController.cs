using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAME_ADS_STUDIO_API.Business.Organization;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using GAME_ADS_STUDIO_API.Configuration;
using Microsoft.AspNetCore.Authorization;
using GAME_ADS_STUDIO_API.Models.Organization;

namespace GAME_ADS_STUDIO_API.Controllers
{
    [Route("/api/organizations")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationBusinessLogic _business;

        public OrganizationController(GasContext context, IOptions<AppSettings> appSettings)
        {
            _business = new OrganizationBusinessLogic(context, appSettings);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult getOrganizations([FromQuery] int offset, [FromQuery] int limit)
        {
            return Ok(_business.GetOrganizations(offset, limit));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetOrganizationById(int id)
        {
            var organizationGet = _business.GetOrganizationById(id);

            if (organizationGet != null)
                return Ok(organizationGet);
            if (id < 0)
                return BadRequest();
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
        public IActionResult Patch(int id, [FromForm] OrganizationUpdateModel newOrganization)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateOrganizationById(id, newOrganization);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => NotFound(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] OrganizationUpdateModel newOrganization)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateOrganizationById(id, newOrganization);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => NotFound(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _business.DeleteOrganizationById(id);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpPost("{id}/users/{userId}")]
        public IActionResult AddUserToOrganization(int id, int userId)
        {
            var success = _business.AddUserToOrganization(id, userId);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpGet("{id}/users")]
        public IActionResult GetOrganizationUsers(int id)
        {
            var users = _business.GetOrganizationUsers(id);

            if (users != null)
                return Ok(users);
            if (id < 0)
                return BadRequest();
            return NotFound("Organization not found.");
        }

        [AllowAnonymous]
        [HttpDelete("{id}/users/{userId}")]
        public IActionResult DeleteUserFromOrganization(int id, int userId)
        {
            var success = _business.DeleteUserFromOrganization(id, userId);

            return success switch
            {
                1 => (IActionResult)Ok("Deleted user from organization successfully"),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
