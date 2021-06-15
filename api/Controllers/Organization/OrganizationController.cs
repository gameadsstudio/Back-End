using api.Business.Organization;
using api.Models.Organization;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<GetDto<object>> GetOrganization(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<object>(_business.GetOrganizationById(id, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<OrganizationPublicDto>> GetAll([FromQuery] PagingDto paging, [FromQuery] OrganizationFiltersDto filters)
        {
            return Ok(new GetAllDto<OrganizationPublicDto>(_business.GetOrganizations(paging, filters)));
        }

        [HttpPost]
        public ActionResult<GetDto<OrganizationPrivateDto>> Post([FromForm] OrganizationCreationDto newOrganization)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Created("Organization", new GetDto<OrganizationPrivateDto>(_business.AddNewOrganization(newOrganization, currentUser)));
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<OrganizationPrivateDto>> Patch(string id, [FromForm] OrganizationUpdateDto newOrganization)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<OrganizationPrivateDto>(_business.UpdateOrganizationById(id, newOrganization, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.DeleteOrganizationById(id, currentUser);

            return Ok();
        }

        [HttpPost("{id}/users/{userId}")]
        public ActionResult<GetDto<OrganizationPrivateDto>> AddUserToOrganization(string id, string userId)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<OrganizationPrivateDto>(_business.AddUserToOrganization(id, userId, currentUser)));
        }

        [HttpGet("{id}/users")]
        public ActionResult<GetAllDto<UserPublicDto>> GetOrganizationUsers(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            var users = _business.GetOrganizationUsers(id, currentUser);

            return Ok(new GetAllDto<UserPublicDto>((1, users.Count, 1, users)));
        }

        [HttpDelete("{id}/users/{userId}")]
        public ActionResult<GetDto<OrganizationPrivateDto>> DeleteUserFromOrganization(string id, string userId)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<OrganizationPrivateDto>(_business.DeleteUserFromOrganization(id, userId, currentUser)));
        }
    }
}