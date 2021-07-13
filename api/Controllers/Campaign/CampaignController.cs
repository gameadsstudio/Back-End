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
            return Ok(
                new GetDto<CampaignPublicDto>(_business.GetCampaignById(id))
            );
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging, [FromQuery] CampaignFiltersDto filters)
        {
            return Ok(
                new GetAllDto<CampaignPublicDto>(
                    _business.GetCampaigns(paging, filters)
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
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.AddNewCampaign(newCampaign);

            if (success != null)
                return Created("Campaign", success);
            return Conflict(new { message = "Couldn't create Campaign" });
        }

		[AllowAnonymous]
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] CampaignUpdateModel newCampaign)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateCampaignById(id, newCampaign));
        }

		[AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromForm] CampaignUpdateModel newCampaign)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateCampaignById(id, newCampaign));
        }

		[AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var success = _business.DeleteCampaignById(id);

            return success switch
            {
                1 => Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
