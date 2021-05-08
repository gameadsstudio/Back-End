using System.Linq;
using System.Security.Claims;
using api.Business.Tag;
using api.Helpers;
using api.Models.Common;
using api.Models.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Tag
{
    [Route("/v1/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagBusinessLogic _business;

        public TagController(ITagBusinessLogic business)
        {
            _business = business;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<GetDto<TagPublicDto>> Get(string id)
        {
            return Ok(new GetDto<TagPublicDto>()
            {
                Data = _business.GetTagById(id)
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<GetAllDto<TagPublicDto>> GetAll(
            [FromQuery] PagingDto paging,
            [FromQuery] bool noPaging,
            [FromQuery] TagFiltersDto filters)
        {
            var (page, pageSize, maxPage, tags) = _business.GetTags(paging, filters, noPaging);

            return Ok(new GetAllDto<TagPublicDto>()
            {
                Data =
                {
                    PageIndex = page,
                    ItemsPerPage = pageSize,
                    TotalPages = maxPage,
                    CurrentItemCount = tags.Count,
                    Items = tags
                }
            });
        }

        [HttpPost]
        public ActionResult<GetDto<TagPublicDto>> Post([FromForm] TagCreationDto newTag)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var tag = _business.AddNewTag(newTag, currentUser);
            return Created("tag", new GetDto<TagPublicDto>()
            {
                Data = tag,
            });
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<TagPublicDto>> Patch(string id, [FromForm] TagUpdateDto newTag)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(new GetDto<TagPublicDto>()
            {
                Data = _business.UpdateTagById(id, newTag, currentUser)
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<TagPublicDto> Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            _business.DeleteTagById(id, currentUser);
            return Ok();
        }
    }
}