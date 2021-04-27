using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enum.AdContainer;
using api.Models.Organization;
using api.Models.Tag;

namespace api.Models.AdContainer
{
    [Table("ad_container")]
    public class AdContainerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        /*
          Todo: implement when versions are done
        [Required]
        [ForeignKey("VersionId")]
        public VersionModel Version { get; set; }
        */

        [Required]
        [ForeignKey("OrgId")]
        public OrganizationModel Organization { get; set; }

        public ICollection<TagModel> Tags { get; set; }

        [Required]
        public AdContainerType Type { get; set; }

        public AdContainerAspectRatio AspectRatio { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}