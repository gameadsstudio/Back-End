using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Organization
{
    public class OrganizationModel
    {
        public uint org_id { get; set; }
        public uint media_id { get; set; }
        public string org_name { get; set; }
        public string org_email { get; set; }
        public string org_email_private { get; set; }
        public string org_city { get; set; }
        public string org_address { get; set; }
        public string org_url { get; set; }
        public string org_type { get; set; }
        public string org_status { get; set; }
        public int org_level_default { get; set; }
        public DateTimeOffset org_date_status { get; set; }
        public DateTimeOffset org_date_creation { get; set; }
        public DateTimeOffset org_date_update { get; set; }
    }
}
