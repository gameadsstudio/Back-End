using System.Linq;
using System.Security.Claims;
using api.Business.Advertisements;
using api.Business.User;
using api.Helpers;
using api.Models.Advertisement;
using api.Models.User;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Get(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(new {status = 200, advertisement = _business.GetAdvertisementById(id, currentUser)});
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging)
        {
            var result = _business.GetAdvertisements(paging);

            return Ok(new
            {
                status = 200,
                page = result.Item1,
                pagesize = result.Item2,
                maxPage = result.Item3,
                advertisements = result.Item4
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] AdvertisementCreationModel newAdvertisement)
        {
            var advertisement = _business.AddNewAdvertisement(newAdvertisement);

            return Created("Advertisement", new {status = 201, advertisement});
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] AdvertisementUpdateModel newAdvertisement)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(new
            {
                status = "", advertisement = _business.UpdateAdvertisementById(id, newAdvertisement, currentUser),
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            _business.DeleteAdvertisementById(id, currentUser);

            return Ok(new {status = 200,});
        }
    }
}