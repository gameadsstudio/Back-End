using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Game
{
    public class GameCreationDto
    {
        [Required]
        public string OrgId { get; set; }

        [Required]
        public Guid MediaId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTimeOffset DateLaunch { get; set; }
    }
}
