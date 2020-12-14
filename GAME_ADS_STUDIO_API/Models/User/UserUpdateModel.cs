using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.User
{
    public class UserUpdateModel
    {
        [EmailAddress]
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Alias { get; set; }
        public string Phone { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public string Date_status { get; set; }
    }
}
