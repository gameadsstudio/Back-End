using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Post
{
    public class PostUpdateDto
    {
        [BindRequired]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }
    }
}
