using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.Post;

namespace api.Business.Post
{
    public interface IPostBusinessLogic
    {
        PostPublicDto AddNewPost(PostCreationDto newPost);

        PostPublicDto UpdatePostById(Guid id, PostUpdateDto updatedPost);

        void DeletePostById(Guid id);

        PostPublicDto GetPostById(Guid id);

        (int page, int pageSize, int totalItemCount, IList<PostPublicDto> posts) GetPosts(PagingDto paging, PostFiltersDto filters);

        PostModel GetPostModelById(Guid id);
    }
}
