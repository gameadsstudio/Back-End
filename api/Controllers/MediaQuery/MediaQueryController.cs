using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Business.MediaQuery;
using api.Enums.Media;
using api.Helpers;
using api.Models.Common;
using api.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.MediaQuery
{
    [ApiController]
    [Produces("application/json")]
    [Route("/v1/media-query")]
    public class MediaQueryController : ControllerBase
    {
        private readonly IMediaQueryBusinessLogic _business;

        public MediaQueryController(IMediaQueryBusinessLogic business)
        {
            _business = business;
        }

        [HttpGet]
        public ActionResult<GetDto<object>> Get(
            [FromQuery] [Required] string adContainerId,
            [FromQuery] [Required] Engine engine
            )
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<object>(_business.GetMedia(adContainerId, engine, currentUser)));
        }
    }
}