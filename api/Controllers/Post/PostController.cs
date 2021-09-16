using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Business.Post;
using api.Models.Post;
using api.Helpers;
using api.Models.Common;

namespace api.Controllers.Post
{
    [Route("/v1/post")]
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

        [HttpGet]
        public IActionResult GetAll([FromQuery] PagingDto paging, [FromQuery] PostFiltersDto filters)
        {
            return Ok(
                new GetAllDto<PostPublicDto>(
                    _business.GetPosts(paging, filters)
                )
            );
        }

        [HttpPost]
        public IActionResult Post([FromForm] PostCreationDto newPost)
        {
            ConnectedUser currentUser = new ConnectedUser(User.Claims);

            return Created(
                "Post",
                new GetDto<PostPublicDto>(
                    _business.AddNewPost(newPost, currentUser)
                )
            );
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromForm] PostUpdateDto newPost)
        {
            ConnectedUser currentUser = new ConnectedUser(User.Claims);

            return Ok(
                new GetDto<PostPublicDto>(
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
