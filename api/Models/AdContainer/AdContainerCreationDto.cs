using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.DataAnnotations;
using api.Enums.AdContainer;

namespace api.Models.AdContainer
{
    public class AdContainerCreationDto
    {
        [Required]
        public string Name { get; set; }

        /*
            Todo: implement when versions are done
        [Required]
        public string VersionId { get; set; }
        */

        [Required]
        public string OrgId { get; set; }

        [Required]
        public List<string> TagNames { get; set; }

        [RequiredEnumAttribute]
        public AdContainerType Type { get; set; }

        public AdContainerAspectRatio? AspectRatio { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Depth { get; set; }
    }
}