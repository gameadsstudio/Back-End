using System;

namespace api.Models.Post
{
    public class PostPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }
    }
}
