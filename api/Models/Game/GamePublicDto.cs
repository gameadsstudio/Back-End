﻿using System;
using api.Models.Organization;

namespace api.Models.Game
{
    public class GamePublicDto
    {
        public Guid Id { get; set; }

        public Guid MediaId { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public OrganizationPublicDto Organization { get; set; }

        public DateTimeOffset DateLaunch { get; set; }
        
        public Uri MiniatureUrl { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}
