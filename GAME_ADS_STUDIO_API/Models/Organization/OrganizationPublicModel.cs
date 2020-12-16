using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Organization
{
    public class OrganizationPublicModel
    {
        public uint media_id { get; set; }
        public string org_name { get; set; }
        public string org_email { get; set; }
        public string org_city { get; set; }
        public string org_url { get; set; }
        public string org_type { get; set; }
       
    }
}
