using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Media._3D
{
    [Table(("media_3D"))]
    public class Media3DModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("MediaId")]
        public MediaModel Media { get; set; }

        public Uri ModelLink { get; set; }

        public Uri TextureLink { get; set; }

        public Uri NormalMapLink { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}