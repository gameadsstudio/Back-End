using System.Net;
using api.Business.Organization;
using System.Security.Claims;
using api.Configuration;
using api.Contexts;
using api.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using api.Helpers;

namespace api.Controllers.Organization
{
    [Route("/v1/organizations")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationBusinessLogic _business;

        public OrganizationController(IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _business = organizationBusinessLogic;
        }

        [HttpGet("{id}")]
        public IActionResult GetOrganization(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(_business.GetOrganizationById(id, currentUser));
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging)
        {
            var result = _business.GetOrganizations(paging);

            return Ok(new
            {
                status = 200,
                page = result.Item1,
                pagesize = result.Item2,
                maxPage = result.Item3,
                organizations = result.Item4
            });
        }

        [HttpPost]
        public IActionResult Post([FromForm] OrganizationCreationDto newOrganization)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            var organization = _business.AddNewOrganization(newOrganization, currentUser);

            return Created("Organization", new { status = 201, organization });
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] OrganizationUpdateDto newOrganization)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(_business.UpdateOrganizationById(id, newOrganization, currentUser));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            _business.DeleteOrganizationById(id, currentUser);

            return Ok(new { status = 200 });
        }

        [HttpPost("{id}/users/{userId}")]
        public IActionResult AddUserToOrganization(string id, string userId)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(_business.AddUserToOrganization(id, userId, currentUser));
        }

        [HttpGet("{id}/users")]
        public IActionResult GetOrganizationUsers(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            var users = _business.GetOrganizationUsers(id, currentUser);

            if (users != null)
                return Ok(users);
            return NotFound("Organization not found.");
        }

        [HttpDelete("{id}/users/{userId}")]
        public IActionResult DeleteUserFromOrganization(string id, string userId)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            _business.DeleteUserFromOrganization(id, userId, currentUser);

            return Ok(new { status = 200 });
        }
    }
}