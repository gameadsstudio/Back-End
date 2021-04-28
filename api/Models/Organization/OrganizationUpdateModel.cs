using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Campaign;
using api.Models.User;
using api.Models.Game;

namespace api.Models.Organization
{
    public class OrganizationUpdateModel
    {
        public string Name { get; set; }

        [EmailAddress]
        public string PublicEmail { get; set; }
        [EmailAddress]
        public string PrivateEmail { get; set; }
        public string Localization { get; set; }
        public string LogoUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string DefaultAuthorization { get; set; }
    }
}
