using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserRefreshDto
    {
        [StringLength(64)]
        public string RefreshToken { get; set; }
    }
}