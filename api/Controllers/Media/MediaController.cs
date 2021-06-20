using System.ComponentModel.DataAnnotations;
using api.Business.Media;
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
        private readonly IMediaBusinessLogic _business;

        public MediaController(IMediaBusinessLogic business)
        {
            _business = business;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public ActionResult<GetDto<MediaPublicDto>> Post([FromForm] MediaCreationDto newMedia)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Created("medias",new GetDto<MediaPublicDto>(_business.AddNewMedia(newMedia, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<MediaPublicDto>> GetAll(
            [FromQuery] PagingDto paging,
            [FromQuery] [Required] string versionId)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<MediaPublicDto>(_business.GetMedias(paging, versionId, currentUser)));
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<MediaPublicDto>> Get(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<MediaPublicDto>(_business.GetMediaById(id, currentUser)));
        }

        // Todo : Find a better type for the object, matching engine dtos
        // MediaPublicDto.Media -> Containing specified engine DTO
        [HttpGet("{id}/{engine}")]
        public ActionResult<GetDto<MediaPublicDto>> GetEngine(string id, string engine)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<MediaPublicDto>(_business.GetEngineMediaById(id, currentUser, engine)));
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<MediaPublicDto>> Patch(string id,
            [FromForm] MediaUpdateDto updateDto)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<MediaPublicDto>(_business.UpdateMediaById(id, updateDto, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);
            _business.DeleteMediaById(id, currentUser);
            return NoContent();
        }
    }
}