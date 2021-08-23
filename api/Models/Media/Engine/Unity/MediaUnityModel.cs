using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Media.Engine.Unity
{
    public class MediaUnityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("MediaId")]
        public MediaModel Media { get; set; }

        public Uri AssetBundleLink { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}