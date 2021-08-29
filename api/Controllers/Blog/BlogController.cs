using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Business.Blog;
using api.Models.Blog;
using api.Helpers;
using api.Models.Common;

namespace api.Controllers.Campaign
{
    [Route("/v1/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogBusinessLogic _business;

        public BlogController(IBlogBusinessLogic blogBusinessLogic)
        {
            _business = blogBusinessLogic;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(new GetDto<BlogPublicDto>(_business.GetPostById(id)));
        }

        [HttpPost]
        public IActionResult Post([FromForm] BlogCreationDto newPost)
        {
            ConnectedUser currentUser = new ConnectedUser(User.Claims);

            return Created(
                "Post",
                new GetDto<BlogPublicDto>(
                    _business.AddNewPost(newPost, currentUser)
                )
            );
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromForm] BlogUpdateDto newPost)
        {
            ConnectedUser currentUser = new ConnectedUser(User.Claims);

            return Ok(
                new GetDto<BlogPublicDto>(
                    _business.UpdatePostById(id, newPost, currentUser)
                )
            );
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ConnectedUser currentUser = new ConnectedUser(User.Claims);

            _business.DeletePostById(id, currentUser);
            return Ok();
        }
    }
}
