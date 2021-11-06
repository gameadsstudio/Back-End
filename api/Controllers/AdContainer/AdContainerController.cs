using System.ComponentModel.DataAnnotations;
using api.Business.AdContainer;
using api.Helpers;
using api.Models.AdContainer;
using api.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.AdContainer
{
    [ApiController]
    [Produces("application/json")]
    [Route("/v1/ad-containers")]
    public class AdContainerController : ControllerBase
    {
        private readonly IAdContainerBusinessLogic _business;

        public AdContainerController(IAdContainerBusinessLogic business)
        {
            _business = business;
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<AdContainerPublicDto>> Get(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<AdContainerPublicDto>(_business.GetAdContainerById(id, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<AdContainerPublicDto>> GetAll([FromQuery] PagingDto paging,
            [FromQuery] AdContainerFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<AdContainerPublicDto>(_business.GetAdContainers(paging, filters, currentUser)));
        }

        [HttpPost]
        public ActionResult<GetDto<AdContainerPublicDto>> Post([FromForm] AdContainerCreationDto newAdContainer)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Created("ad-container",
                new GetDto<AdContainerPublicDto>(_business.AddNewAdContainer(newAdContainer, currentUser)));
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<AdContainerPublicDto>> Patch(string id,
            [FromForm] AdContainerUpdateDto newAdContainer)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(
                new GetDto<AdContainerPublicDto>(_business.UpdateAdContainerById(id, newAdContainer, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.DeleteAdContainerById(id, currentUser);

            return Ok();
        }
    }
}