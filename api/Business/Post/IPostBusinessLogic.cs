using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.Post;

namespace api.Business.Post
{
    public interface IPostBusinessLogic
    {
        PostPublicDto AddNewPost(PostCreationDto newPost, ConnectedUser currentUser);

        PostPublicDto UpdatePostById(Guid id, PostUpdateDto updatedPost, ConnectedUser currentUser);

        void DeletePostById(Guid id, ConnectedUser currentUser);

        PostPublicDto GetPostById(Guid id);

        (int page, int pageSize, int maxPage, IList<PostPublicDto> posts) GetPosts(PagingDto paging, PostFiltersDto filters);

        PostModel GetPostModelById(Guid id);
    }
}
