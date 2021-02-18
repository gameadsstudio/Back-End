using api.Business.Campaign;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers.Campaign
{
    [Route("/campaigns")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignBusinessLogic _business;

        public CampaignController(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _business = new CampaignBusinessLogic(context, appSettings);
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
