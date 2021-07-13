using System;
using api.Business.Campaign;
using api.Models.Campaign;
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

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<CampaignPublicDto>(_business.GetCampaignById(id, currentUser)));
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging, [FromQuery] CampaignFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<CampaignPublicDto>(_business.GetCampaigns(paging, filters, currentUser)));
        }

        [HttpPost]
        public IActionResult Post([FromForm] CampaignCreationDto newCampaign)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<CampaignPublicDto>(_business.AddNewCampaign(newCampaign, currentUser)));
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromForm] CampaignUpdateDto newCampaign)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<CampaignPublicDto>(_business.UpdateCampaignById(id, newCampaign, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.DeleteCampaignById(id, currentUser);

            return Ok();
        }
    }
}