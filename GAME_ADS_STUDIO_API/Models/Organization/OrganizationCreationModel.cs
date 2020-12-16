﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Organization
{
    public class OrganizationCreationModel
    {
        [Required]
        public string org_name { get; set; }

        [Required]
        [EmailAddress]
        public string org_email_private { get; set; }

        [Required]
        public string org_type { get; set; }
    }
}
