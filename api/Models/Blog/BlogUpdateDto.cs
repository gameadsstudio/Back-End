using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Blog
{
    public class BlogUpdateDto
    {
        [BindRequired]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }
    }
}
