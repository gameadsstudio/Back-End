using System;
using System.Collections.Generic;
using api.Models.Post;

namespace api.Repositories.Post
{
    public interface IPostRepository
    {
        PostModel AddNewPost(PostModel post);
        PostModel UpdatePost(PostModel post);
        void DeletePost(PostModel post);
        PostModel GetPostById(Guid id);
        (IList<PostModel>, int) GetPosts(int offset, int limit);
    }
}
