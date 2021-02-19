using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Game
{
    [Table("game")]
    public class GameModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column("org_id")]
        [ForeignKey("OrganizationModel")]
        public Guid OrganizationId { get; set; }
        [Column("media_id")]
        public Guid MediaId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("date_launch")]
        public DateTimeOffset DateLaunch { get; set; }
        [Column("date_creation")]
        public DateTimeOffset DateCreation { get; set; }
        [Column("date_update")]
        public DateTimeOffset DateUpdate { get; set; }
    }
}
