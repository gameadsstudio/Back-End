using api.Business.Campaign;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] CampaignUpdateModel newCampaign)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateCampaignById(id, newCampaign));
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromForm] CampaignUpdateModel newCampaign)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateCampaignById(id, newCampaign));
        }

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
