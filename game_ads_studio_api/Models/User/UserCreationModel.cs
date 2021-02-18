using System.ComponentModel.DataAnnotations;

namespace game_ads_studio_api.Models.User
{
    public class UserCreationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

    }
}
