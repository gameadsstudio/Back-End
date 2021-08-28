using System;

namespace api.Models.Blog
{
    public class BlogPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }
    }
}
