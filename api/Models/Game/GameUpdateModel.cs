using System;

namespace api.Models.Game
{
    public class GameUpdateModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string OrganizationId { get; set; }
        public string MediaId { get; set; }
        public DateTimeOffset DateLaunch { get; set; }
    }
}
