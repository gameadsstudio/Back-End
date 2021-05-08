using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
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
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(new GetDto<AdContainerPublicDto>()
            {
                Data = _business.GetAdContainerById(id, currentUser)
            });
        }

        [HttpGet]
        public ActionResult<GetAllDto<AdContainerPublicDto>> GetAll(
            [FromQuery] PagingDto paging,
            [FromQuery] [Required] string orgId
            )
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var (page, pageSize, maxPage, adContainers) = _business.GetAdContainers(paging, orgId, currentUser);

            return Ok(new GetAllDto<AdContainerPublicDto>()
            {
                Data =
                {
                    PageIndex = page,
                    ItemsPerPage = pageSize,
                    TotalPages = maxPage,
                    CurrentItemCount = adContainers.Count,
                    Items = adContainers
                }
            });
        }

        [HttpPost]
        public ActionResult<GetDto<AdContainerPublicDto>> Post([FromForm] AdContainerCreationDto newAdContainer)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var adContainer = _business.AddNewAdContainer(newAdContainer, currentUser);
            return Created("ad-container", new GetDto<AdContainerPublicDto>()
            {
                Data = adContainer
            });
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<AdContainerPublicDto>> Patch(string id, [FromForm] AdContainerUpdateDto newAdContainer)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(new GetDto<AdContainerPublicDto>()
            {
                Data = _business.UpdateAdContainerById(id, newAdContainer, currentUser)
            });
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