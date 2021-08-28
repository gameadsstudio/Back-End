using System;
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

        BlogModel GetBlogModelById(Guid id);
    }
}
