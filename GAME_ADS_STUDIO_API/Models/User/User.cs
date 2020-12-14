﻿using System;

namespace GAME_ADS_STUDIO_API.Models.User
{
    public class UserModel
    {
        public uint Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Alias { get; set; }
        public string Phone { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public string Date_status { get; set; }
        public DateTimeOffset Date_creation { get; set; }
        public DateTimeOffset Date_update { get; set; }
    }
}
