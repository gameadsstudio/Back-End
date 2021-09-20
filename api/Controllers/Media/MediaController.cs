using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Business.Media;
using api.Enums.Media;
using api.Helpers;
using api.Models.Common;
using api.Models.Media;
using api.Models.Media.Engine.Unity;
using Microsoft.AspNetCore.Authorization;
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
        public ActionResult<GetDto<MediaPublicDto>> Post([FromForm] MediaCreationDto newMedia)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Created("medias",new GetDto<MediaPublicDto>(_business.AddNewMedia(newMedia, currentUser)));
        }

        [HttpPost("{id}/retry")]
        public ActionResult<GetDto<MediaPublicDto>> RetryBuild(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Created("medias",new GetDto<MediaPublicDto>(_business.RetryBuild(id, currentUser)));
        }

        // TODO: remove allow anonymous
        [AllowAnonymous]
        [HttpPost("{id}/unity")]
        public ActionResult<GetDto<MediaUnityPublicDto>> Post([FromForm] MediaUnityCreationDto newMediaUnity, string id)
        {
            return Created("medias",new GetDto<MediaUnityPublicDto>(_business.AddNewMediaUnity(newMediaUnity, id)));
        }

        [AllowAnonymous]
        [HttpPut("{mediaId}/unity/{id}/state")]
        public ActionResult<GetDto<MediaUnityPublicDto>> Put([FromForm] MediaState newState, string id, string mediaId)
        {
            return Ok(new GetDto<MediaUnityPublicDto>(_business.UpdateMediaUnityState(newState, id, mediaId)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<MediaPublicDto>> GetAll(
            [FromQuery] PagingDto paging,
            [FromQuery] [Required] string orgId,
            [FromQuery] IList<string> tagNames)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetAllDto<MediaPublicDto>(_business.GetMedias(paging, tagNames, orgId, currentUser)));
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
        public ActionResult<GetDto<MediaPublicDto>> GetEngine(string id, Engine engine)
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

        [HttpPut("{mediaId}/state")]
        public ActionResult<GetDto<MediaPublicDto>> Put([FromForm] MediaState newState, string mediaId)
        {
            return Ok(new GetDto<MediaPublicDto>(_business.UpdateMediaState(newState, mediaId)));
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