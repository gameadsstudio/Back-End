using System;
using api.Business.Organization;
using api.Business.Stripe;
using api.Models.Organization;
using Microsoft.AspNetCore.Mvc;
using api.Helpers;
using api.Models.Common;
using api.Models.User;
using api.Models.Stripe;

namespace api.Controllers.Organization
{
    [Route("/v1/organizations")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationBusinessLogic _business;

        private readonly IStripeBusinessLogic _businessStripe;

        public OrganizationController(IOrganizationBusinessLogic organizationBusinessLogic, IStripeBusinessLogic stripeBusinessLogic)
        {
            _business = organizationBusinessLogic;
            _businessStripe = stripeBusinessLogic;
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
            var organizationResponse = _business.AddNewOrganization(
                newOrganization,
                currentUser
            );
            var customer = _businessStripe.CreateAccount(
                organizationResponse.Name,
                organizationResponse.Email,
                "Organization"
            );
            var organization = _business.GetOrganizationModelById(
                organizationResponse.Id
            );

            organization.StripeAccount = customer.Id;
            return Created("Organization", new GetDto<OrganizationPrivateDto>(organizationResponse));
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
        public ActionResult<GetAllDto<UserPublicDto>> GetOrganizationUsers(string id, [FromQuery] PagingDto paging)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<UserPublicDto>(_business.GetOrganizationUsers(id, paging, currentUser)));
        }

        [HttpDelete("{id}/users/{userId}")]
        public ActionResult<GetDto<OrganizationPrivateDto>> DeleteUserFromOrganization(string id, string userId)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<OrganizationPrivateDto>(_business.DeleteUserFromOrganization(id, userId, currentUser)));
        }

        [HttpPost("{id}/create-session")]
        public IActionResult CreateSession(Guid id, [FromForm] SessionDto session)
        {
            var currentUser = new ConnectedUser(User.Claims);
            var organization = _business.GetOrganizationById(
                id.ToString(),
                currentUser
            );

            session.Customer = _business.GetOrganizationModelById(
                id
            ).StripeAccount;
            return Ok(_businessStripe.CreateSession(session));
        }

        [HttpPost("{id}/add-money")]
        public IActionResult AddMoney(Guid id, [FromForm] PaymentDto payment)
        {
            var currentUser = new ConnectedUser(User.Claims);
            var organization = _business.GetOrganizationById(
                id.ToString(),
                currentUser
            );
            var charge = _businessStripe.CheckChargeComplete(payment.Id);

            if (!charge.Item1) {
                return BadRequest();
            }
            _business.AddMoney(id, charge.Item2);
            return Ok();
        }
    }
}
