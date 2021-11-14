using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Organization;
using api.Models.Version;

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
        
        public Uri MiniatureUrl { get; set; }
        
        [ForeignKey("OrgId")]
        public OrganizationModel Organization { get; set; }
        
        public ICollection<VersionModel> Versions { get; set; }
    }
}
