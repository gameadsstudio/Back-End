using System;
using System.Linq;
using api.Contexts;
using api.Models.Blog;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Blog
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApiContext _context;

        public BlogRepository(ApiContext context)
        {
            _context = context;
        }

        public BlogModel AddNewPost(BlogModel post)
        {
            _context.Blog.Add(post);
            _context.SaveChanges();
            return post;
        }

        public BlogModel UpdatePost(BlogModel post)
        {
            _context.Update(post);
            _context.SaveChanges();
            return post;
        }

        public void DeletePost(BlogModel post)
        {
            _context.Blog.Remove(post);
            _context.SaveChanges();
        }

        public BlogModel GetPostById(Guid id)
        {
            return _context.Blog
                .Include(x => x.Id)
                .SingleOrDefault(post => post.Id == id);
        }
    }
}
