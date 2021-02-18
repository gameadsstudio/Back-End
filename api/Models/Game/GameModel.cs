using System;

namespace api.Models.Game
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public Guid OrgId { get; set; }
        public Guid MediaId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTimeOffset DateLaunch { get; set; }
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
    }
}
