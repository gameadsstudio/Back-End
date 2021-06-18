using System.Collections.Generic;
using api.Enums.AdContainer;

namespace api.Models.AdContainer
{
    public class AdContainerUpdateDto
    {
        public string Name { get; set; }

        public string VersionId { get; set; }

        public IList<string> TagNames { get; set; }

        public AdContainerType? Type { get; set; }

        public AdContainerAspectRatio? AspectRatio { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Depth { get; set; }
    }
}