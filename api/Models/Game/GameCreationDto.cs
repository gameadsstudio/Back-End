using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Game
{
    public class GameCreationDto
    {
        [BindRequired]
        public Guid OrgId { get; set; }

        [Required]
        public Guid MediaId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTimeOffset DateLaunch { get; set; }
        
        public IFormFile Miniature { get; set; }
    }
}
