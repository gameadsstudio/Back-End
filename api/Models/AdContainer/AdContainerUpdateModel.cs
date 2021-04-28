using System.Collections.Generic;
using api.Enum.AdContainer;
using api.Models.Tag;

namespace api.Models.AdContainer
{
    public class AdContainerUpdateModel
    {
        public string Name { get; set; }

        /*
            Todo: implement when versions are done
        public string VersionId { get; set; }
        */

        public string OrgId { get; set; }

        public List<string> Tags { get; set; }

        public AdContainerType? Type { get; set; }

        public AdContainerAspectRatio? AspectRatio { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Depth { get; set; }
    }
}