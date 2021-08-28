using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Blog
{
    public class BlogCreationDto
    {
        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }
    }
}
