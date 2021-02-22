using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Game
{
    [Table("game")]
    public class GameModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column("org_id")]
        [ForeignKey("OrganizationModel")]
        public Guid OrganizationId { get; set; }
        public Guid MediaId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTimeOffset DateLaunch { get; set; }
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
    }
}
