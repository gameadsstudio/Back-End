using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserCreationModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string Username { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public string Password { get; set; }
    }
}