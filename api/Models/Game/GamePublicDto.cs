using System;
namespace api.Models.Game
{
    public class GamePublicDto : IGameDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
