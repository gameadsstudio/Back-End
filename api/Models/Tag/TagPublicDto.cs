using System;

namespace api.Models.Tag
{
    public class TagPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}