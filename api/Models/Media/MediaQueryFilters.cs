using System;
using System.Collections.Generic;
using api.Models.Tag;

namespace api.Models.Media
{
    public class MediaQueryFilters
    {
        public readonly Enums.Media.Engine Engine;

        public readonly IEnumerable<Guid> MediaIds;

        public readonly ICollection<TagModel> Tags;

        public MediaQueryFilters(Enums.Media.Engine engine, IEnumerable<Guid> mediaIds, ICollection<TagModel> tags)
        {
            Engine = engine;
            MediaIds = mediaIds;
            Tags = tags;
        }
    }
}