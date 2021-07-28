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
        public IActionResult Get(Guid id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<AdvertisementPublicDto>(_business.GetAdvertisementById(id, currentUser)));
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging, [FromQuery] AdvertisementFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);
            var result = _business.GetAdvertisements(paging, filters, currentUser);

            return Ok(new
            {
                status = 200,
                page = result.Item1,
                pagesize = result.Item2,
                maxPage = result.Item3,
                advertisements = result.Item4
            });
        }

        [HttpPost]
        public IActionResult Post([FromForm] AdvertisementCreationDto newAdvertisement)
        {
            var currentUser = new ConnectedUser(User.Claims);
            
            var advertisement = _business.AddNewAdvertisement(newAdvertisement, currentUser);

            return Created("Advertisement", new {status = 201, advertisement});
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromForm] AdvertisementUpdateDto newAdvertisement)
        {
            var currentUser = new ConnectedUser(User.Claims);
            return Ok(new
            {
                status = "", advertisement = _business.UpdateAdvertisementById(id, newAdvertisement, currentUser),
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.DeleteAdvertisementById(id, currentUser);

            return Ok(new {status = 200,});
        }
    }
}