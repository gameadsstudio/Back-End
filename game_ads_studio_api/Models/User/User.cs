using System;

namespace game_ads_studio_api.Models.User
{
    // TODO : check if username is needed
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Alias { get; set; }
        public string Phone { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public string DateStatus { get; set; }
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
    }
}
