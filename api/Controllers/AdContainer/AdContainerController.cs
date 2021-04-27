using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using api.Business.AdContainer;
using api.Helpers;
using api.Models.AdContainer;
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
        public ActionResult<AdContainerModel> Get(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(_business.GetAdContainerById(id, currentUser));
        }

        [HttpGet]
        public ActionResult<IEnumerable<AdContainerModel>> GetAll(
            [FromQuery] PagingDto paging,
            [FromQuery] [Required] string orgId
            )
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var (page, pagesize, maxPage, adContainers) = _business.GetAdContainers(paging, orgId, currentUser);

            return Ok(new
            {
                status = 200,
                page,
                pagesize,
                maxPage,
                adContainers
            });
        }

        [HttpPost]
        public ActionResult<AdContainerModel> Post([FromForm] AdContainerCreationModel newAdContainer)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var success = _business.AddNewAdContainer(newAdContainer, currentUser);
            return Created("ad-container", success);
        }

        [HttpPatch("{id}")]
        public ActionResult<AdContainerModel> Patch(string id, [FromForm] AdContainerUpdateModel newAdContainer)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(_business.UpdateAdContainerById(id, newAdContainer, currentUser));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            _business.DeleteAdContainerById(id, currentUser);
            return Ok();
        }
    }
}