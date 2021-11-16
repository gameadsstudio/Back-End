using api.Models.AdContainer;

namespace api.Models.Media
{
    public class MediaQueryFilters
    {
        public readonly Enums.Media.Engine Engine;

        public readonly AdContainerModel AdContainer;

        public MediaQueryFilters(Enums.Media.Engine engine, AdContainerModel adContainer)
        {
            Engine = engine;
            AdContainer = adContainer;
        }
    }
}