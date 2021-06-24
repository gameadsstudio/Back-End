using System;
using api.Business.Campaign;
using api.Models.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get(Guid id)
        {
            // return Ok(new GetDto<NAME>(_business.GetCampaignById(id)));
            return Ok(_business.GetCampaignById(id));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get([FromQuery] CampaignFiltersDto filters)
        {
            return Ok(
                _business.GetAll(filters)
            );
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] CampaignCreationDto newCampaign)
        {
            CampaignPublicDto success = null;

            if (!ModelState.IsValid) {
                return BadRequest();
            }
            success = _business.AddNewCampaign(newCampaign);
            if (success != null) {
                return Created("Campaign", success);
            }
            return Conflict(new {message = "Couldn't create Campaign"});
        }

        [AllowAnonymous]
        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromForm] CampaignUpdateDto newCampaign)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            return Ok(
                _business.UpdateCampaignById(id, newCampaign)
            );
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromForm] CampaignUpdateDto newCampaign)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            return Ok(_business.UpdateCampaignById(id, newCampaign));
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_business.DeleteCampaignById(id));
        }

    }
}
