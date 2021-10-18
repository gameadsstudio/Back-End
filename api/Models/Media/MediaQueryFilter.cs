using System.Collections.Generic;
using api.Enums.Media;
using api.Models.Tag;

namespace api.Models.Media
{
    public abstract class MediaQueryFilters
    {
        public readonly Enums.Media.Engine Engine;
        public readonly IList<TagPublicDto> Tags;
        public readonly Type Type;

        protected MediaQueryFilters(IList<TagPublicDto> tags, Enums.Media.Engine engine, Type type)
        {
            Tags = tags;
            Type = type;
            Engine = engine;
        }
    }
}