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
            return Ok();
        }

        [HttpPost("unity")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult PostUnity()
        {
            return Ok();
        }
    }
}