using api.Business.Organization;
using System.Security.Claims;
using api.Models.Organization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using api.Helpers;
using api.Models.Common;
using api.Models.User;

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
        public ActionResult<GetDto<IOrganizationDto>> GetOrganization(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(new GetDto<IOrganizationDto>()
            {
                Data = _business.GetOrganizationById(id, currentUser)
            });
        }

        [HttpGet]
        public ActionResult<GetAllDto<OrganizationPublicDto>> GetAll([FromQuery] PagingDto paging, [FromQuery] OrganizationFiltersDto filters)
        {
            var (page, pageSize, maxPage, organizations) = _business.GetOrganizations(paging, filters);

            return Ok(new GetAllDto<OrganizationPublicDto>()
            {
                Data =
                {
                    PageIndex = page,
                    ItemsPerPage = pageSize,
                    TotalPages = maxPage,
                    CurrentItemCount = organizations.Count,
                    Items = organizations
                }
            });
        }

        [HttpPost]
        public ActionResult<GetDto<OrganizationPrivateDto>> Post([FromForm] OrganizationCreationDto newOrganization)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            var organization = _business.AddNewOrganization(newOrganization, currentUser);

            return Created("Organization", new GetDto<OrganizationPrivateDto>()
            {
                Data = organization
            });
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<OrganizationPrivateDto>> Patch(string id, [FromForm] OrganizationUpdateDto newOrganization)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(new GetDto<OrganizationPrivateDto>()
            {
                Data = _business.UpdateOrganizationById(id, newOrganization, currentUser)
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            _business.DeleteOrganizationById(id, currentUser);

            return Ok();
        }

        [HttpPost("{id}/users/{userId}")]
        public ActionResult<GetDto<OrganizationPrivateDto>> AddUserToOrganization(string id, string userId)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(new GetDto<OrganizationPrivateDto>()
            {
                Data = _business.AddUserToOrganization(id, userId, currentUser)
            });
        }

        [HttpGet("{id}/users")]
        public ActionResult<GetAllDto<UserPublicDto>> GetOrganizationUsers(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            var users = _business.GetOrganizationUsers(id, currentUser);
            
            return Ok(new GetAllDto<UserPublicDto>()
            {
                Data =
                {
                    PageIndex = 1,
                    ItemsPerPage = users.Count,
                    TotalPages = 1,
                    CurrentItemCount = users.Count,
                    Items = users,
                }
            });
        }

        [HttpDelete("{id}/users/{userId}")]
        public ActionResult<GetDto<OrganizationPrivateDto>> DeleteUserFromOrganization(string id, string userId)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            var organization = _business.DeleteUserFromOrganization(id, userId, currentUser);

            return Ok(new GetDto<OrganizationPrivateDto>()
            {
                Data = organization,
            });
        }
    }
}