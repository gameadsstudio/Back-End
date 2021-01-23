using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAME_ADS_STUDIO_API.Business.Campaign;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using GAME_ADS_STUDIO_API.Configuration;
using Microsoft.AspNetCore.Authorization;
using GAME_ADS_STUDIO_API.Models.Campaign;

namespace Campaign_ADS_STUDIO_API.Controllers.Campaign
{
    [Route("/api/campaign")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignBusinessLogic _business;

        public CampaignController(IOptions<AppSettings> appSettings)
        {
            _business = new CampaignBusinessLogic(appSettings);
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
        public IActionResult Patch(int id, [FromForm] CampaignUpdateModel newCampaign)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateCampaignById(id, newCampaign);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => NotFound(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] CampaignUpdateModel newCampaign)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateCampaignById(id, newCampaign);

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
            var success = _business.DeleteCampaignById(id);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }

    }
}
