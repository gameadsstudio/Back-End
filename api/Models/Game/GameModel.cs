using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Organization;

namespace api.Models.Game
{
    [Table("game")]
    public class GameModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // TODO: use foreign key on media implementation
        public Guid MediaId { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public DateTimeOffset DateLaunch { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }

        [Required]
        [ForeignKey("OrgId")]
        public OrganizationModel Organization { get; set; }
    }
}
