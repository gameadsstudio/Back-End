using System.Collections.Generic;
using api.Enums.Media;

namespace api.Models.AdContainer
{
    public class AdContainerUpdateDto
    {
        public string Name { get; set; }

        public string VersionId { get; set; }

        public IList<string> TagNames { get; set; }

        public Type? Type { get; set; }

        public AspectRatio? AspectRatio { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Depth { get; set; }
    }
}