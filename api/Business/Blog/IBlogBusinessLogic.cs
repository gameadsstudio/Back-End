using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.Blog;

namespace api.Business.Blog
{
    public interface IBlogBusinessLogic
    {
        BlogPublicDto AddNewPost(BlogCreationDto newPost, ConnectedUser currentUser);

        BlogPublicDto UpdatePostById(Guid id, BlogUpdateDto updatedPost, ConnectedUser currentUser);

        void DeletePostById(Guid id, ConnectedUser currentUser);

        BlogPublicDto GetPostById(Guid id);

        (int page, int pageSize, int maxPage, IList<BlogPublicDto> posts) GetPosts(PagingDto paging, BlogFiltersDto filters);

        BlogModel GetBlogModelById(Guid id);
    }
}
