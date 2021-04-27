using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models.Tag;

namespace api.Models.AdContainer
{
    public class AdContainerCreationModel
    {
        [Required]
        public string Name { get; set; }

        /*
            Todo: implement when versions are done
        [Required]
        public string VersionId { get; set; }
        */

        public string OrgId { get; set; }

        [Required]
        public List<TagModel> Tags { get; set; }

        [Required]
        public AdContainerType Type { get; set; }

        public AdContainerAspectRatio? AspectRatio { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Depth { get; set; }
    }
}