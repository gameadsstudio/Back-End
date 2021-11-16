using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Game;

namespace api.Models.Version
{
    public class VersionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public GameModel Game { get; set; }
        
        public string Name { get; set; }
        
        public string SemVer { get; set; }
        
        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}