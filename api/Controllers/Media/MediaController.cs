using api.Helpers;
using api.Models.Common;
using api.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Media
{
    [ApiController]
    [Produces("application/json")]
    [Route("/v1/medias")]
    public class MediaController : ControllerBase
    {
        public MediaController()
        {
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public ActionResult<GetDto<MediaPublicDto>> Post([FromForm] MediaCreationDto newMedia)
        {
            return Created("","");
        }

        [HttpGet]
        public ActionResult<GetAllDto<MediaPublicDto>> GetAll(
            [FromQuery] PagingDto paging)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<MediaPublicDto>> Get(string id)
        {
            return Ok();
        }

        // Todo : Find a better type for the object, matching engine dtos
        // MediaPublicDto.Media -> Containing specified engine DTO
        [HttpGet("{id}/{engine}")]
        public ActionResult<GetDto<MediaPublicDto>> GetEngine(string id, string engine)
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<MediaPublicDto>> Patch(string id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return NoContent();
        }
    }
}