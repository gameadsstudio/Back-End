using System.Linq;
using System.Security.Claims;
using api.Business.Tag;
using api.Helpers;
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
        public IActionResult Get(string id)
        {
            return Ok(_business.GetTagById(id));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] PagingDto paging,
            [FromQuery] bool noPaging,
            [FromQuery] TagFiltersDto filters)
        {
            var result = _business.GetTags(paging, filters, noPaging);

            return Ok(new
            {
                status = 200,
                page = result.Item1,
                pagesize = result.Item2,
                maxPage = result.Item3,
                tags = result.Item4
            });
        }

        [HttpPost]
        public IActionResult Post([FromForm] TagCreationDto newTag)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var success = _business.AddNewTag(newTag, currentUser);
            return Created("tag", success);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] TagUpdateDto newTag)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(_business.UpdateTagById(id, newTag, currentUser));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            _business.DeleteTagById(id, currentUser);
            return Ok();
        }
    }
}