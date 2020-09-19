using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Organization
{
    public class OrganizationModel
    {
        public uint Id { get; set; }
        public string PublicEmail { get; set; }
        public string PrivateEmail { get; set; }
        public string Localization { get; set; }
        public string LogoUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string DefaultAuthorization { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset ModificationDate { get; set; }
    }
}
