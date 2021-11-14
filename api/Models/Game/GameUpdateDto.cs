using System;
using Microsoft.AspNetCore.Http;

namespace api.Models.Game
{
    public class GameUpdateDto
    {
        public string Name { get; set; }
        
        public string Status { get; set; }
        
        public string MediaId { get; set; }
        
        public DateTimeOffset DateLaunch { get; set; }
        
        public IFormFile Miniature { get; set; }
    }
}
