using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.AdContainer;
using api.Models.Advertisement;
using api.Models.Media;

namespace api.Models.Tag
{
    [Table("tag")]
    public class TagModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<AdContainerModel> AdContainers { get; set; }

        public ICollection<AdvertisementModel> Advertisements { get; set; }

        public ICollection<MediaModel> Medias { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}