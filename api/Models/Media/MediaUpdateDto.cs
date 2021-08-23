using System.Collections.Generic;

namespace api.Models.Media
{
    public class MediaUpdateDto
    {
        public IList<string> TagName { get; set; }

        public string Name { get; set; }
    }
}