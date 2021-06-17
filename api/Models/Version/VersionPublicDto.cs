using System;
using api.Models.Game;

namespace api.Models.Version
{
    public class VersionPublicDto
    {
        public Guid Id { get; set; }
        
        public GamePublicDto Game { get; set; }

        public string Name { get; set; }
        
        public string SemVer { get; set; }
    }
}