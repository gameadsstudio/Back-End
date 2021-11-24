using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Business.Post;
using api.Models.Post;
using api.Helpers;
using api.Models.Common;

namespace api.Controllers.Post
{
    [Route("/v1/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostBusinessLogic _business;

        public PostController(IPostBusinessLogic postBusinessLogic)
        {
            _business = postBusinessLogic;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(new GetDto<PostPublicDto>(_business.GetPostById(id)));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging, [FromQuery] PostFiltersDto filters)
        {
            return Ok(
                new GetAllDto<PostPublicDto>(
                    _business.GetPosts(paging, filters)
                )
            );
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        public IActionResult Post([FromForm] PostCreationDto newPost)
        {
            return Created(
                "Post",
                new GetDto<PostPublicDto>(
                    _business.AddNewPost(newPost)
                )
            );
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromForm] PostUpdateDto newPost)
        {
            return Ok(
                new GetDto<PostPublicDto>(
                    _business.UpdatePostById(id, newPost)
                )
            );
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _business.DeletePostById(id);
            return Ok();
        }
    }
}
