using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums.Media;
using api.Models.Advertisement;
using api.Models.Organization;
using api.Models.Tag;
using Type = api.Enums.Media.Type;

namespace api.Models.Media
{
    [Table("media")]
    public class MediaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("OrgId")] public OrganizationModel Organization { get; set; }

        public string Name { get; set; }

        public ICollection<TagModel> Tags { get; set; }
        
        public ICollection<AdvertisementModel> Advertisements { get; set; }

        public Type Type { get; set; }

        public MediaStateEnum State { get; set; }

        public string StateMessage { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}