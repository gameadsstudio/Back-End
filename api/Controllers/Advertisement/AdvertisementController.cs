using System;
using api.Business.Advertisements;
using api.Helpers;
using api.Models.Advertisement;
using api.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Advertisement
{
    [Route("/v1/ads")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementBusinessLogic _business;

        public AdvertisementController(IAdvertisementBusinessLogic advertisementBusinessLogic)
        {
            _business = advertisementBusinessLogic;
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<AdvertisementPublicDto>> Get(Guid id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<AdvertisementPublicDto>(_business.GetAdvertisementById(id, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<AdvertisementPublicDto>> GetAll([FromQuery] PagingDto paging, [FromQuery] AdvertisementFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<AdvertisementPublicDto>(_business.GetAdvertisements(paging, filters, currentUser)));
        }

        [HttpPost]
        public ActionResult<GetDto<AdvertisementPublicDto>> Post([FromForm] AdvertisementCreationDto newAdvertisement)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Created("Advertisement",
                new GetDto<AdvertisementPublicDto>(_business.AddNewAdvertisement(newAdvertisement, currentUser)));
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<AdvertisementPublicDto>> Patch(Guid id, [FromForm] AdvertisementUpdateDto newAdvertisement)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<AdvertisementPublicDto>(
                _business.UpdateAdvertisementById(id, newAdvertisement, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.DeleteAdvertisementById(id, currentUser);

            return Ok();
        }
    }
}
