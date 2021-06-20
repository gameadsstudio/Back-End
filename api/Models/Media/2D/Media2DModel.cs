using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums.Media;

namespace api.Models.Media._2D
{
    [Table(("media_2D"))]
    public class Media2DModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("MediaId")]
        public MediaModel Media { get; set; }

        public AspectRatio AspectRatio { get; set; }

        public string TextureLink { get; set; }

        public string NormalMapLink { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}