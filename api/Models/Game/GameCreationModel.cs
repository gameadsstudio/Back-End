using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Game
{
    public class GameCreationModel
    {
        [Required]
        public string OrganizationId { get; set; }

        [Required]
        public string MediaId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTimeOffset DateLaunch { get; set; }
    }
}
