using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Post
{
    public class PostRepository : IPostRepository
    {
        private readonly ApiContext _context;

        public PostRepository(ApiContext context)
        {
            _context = context;
        }

        public PostModel AddNewPost(PostModel post)
        {
            _context.Post.Add(post);
            _context.SaveChanges();
            return post;
        }

        public PostModel UpdatePost(PostModel post)
        {
            _context.Update(post);
            _context.SaveChanges();
            return post;
        }

        public void DeletePost(PostModel post)
        {
            _context.Post.Remove(post);
            _context.SaveChanges();
        }

        public PostModel GetPostById(Guid id)
        {
            return _context.Post
                .Include(x => x.Id)
                .SingleOrDefault(post => post.Id == id);
        }

        public (IList<PostModel>, int) GetPosts(int offset, int limit)
        {
            var query = _context.Post.OrderByDescending(
                post => post.DateCreation
            );

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }
    }
}
