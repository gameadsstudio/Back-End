using System;
using System.Collections.Generic;
using api.Models.Blog;

namespace api.Repositories.Blog
{
    public interface IBlogRepository
    {
        BlogModel AddNewPost(BlogModel post);
        BlogModel UpdatePost(BlogModel post);
        void DeletePost(BlogModel post);
        BlogModel GetPostById(Guid id);
        (IList<BlogModel>, int) GetPosts(int offset, int limit);
    }
}
