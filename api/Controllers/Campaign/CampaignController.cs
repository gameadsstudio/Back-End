using api.Business.Campaign;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Helpers;
using api.Models.Common;

namespace api.Controllers.Campaign
{
    [Route("/v1/campaigns")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignBusinessLogic _business;

        public CampaignController(ICampaignBusinessLogic campaignBusinessLogic)
        {
            _business = campaignBusinessLogic;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(
                new GetDto<CampaignPublicDto>(
                    _business.GetCampaignById(id, currentUser)
                )
            );
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging, [FromQuery] CampaignFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(
                new GetAllDto<CampaignPublicDto>(
                    _business.GetCampaigns(paging, filters, currentUser)
                )
            );
        }

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Get([FromQuery] CampaignDto settings)
		{
			return Ok(
				_business.GetOrganizationCampaigns(settings.OrganizationId)
			);
		}

		[AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] CampaignCreationModel newCampaign)
        {
            CampaignPublicDto success = null;
            var currentUser = new ConnectedUser(User.Claims);

            if (!ModelState.IsValid) {
                return BadRequest();
            }
            success = _business.AddNewCampaign(newCampaign, currentUser);
            if (success != null) {
                return Created("Campaign", success);
            return Conflict(new { message = "Couldn't create Campaign" });
        }

		[AllowAnonymous]
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] CampaignUpdateModel newCampaign)
        {
            var currentUser = new ConnectedUser(User.Claims);

            if (!ModelState.IsValid) {
                return BadRequest();
            }
            return Ok(
                _business.UpdateCampaignById(id, newCampaign, currentUser)
            );
        }

		[AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromForm] CampaignUpdateModel newCampaign)
        {
            var currentUser = new ConnectedUser(User.Claims);

            if (!ModelState.IsValid) {
                return BadRequest();
            }
            return Ok(
                _business.UpdateCampaignById(id, newCampaign, currentUser)
            );
        }

		[AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(_business.DeleteCampaignById(id, currentUser));
        }
    }
}
