using api.Enums.Media;

namespace api.Models.Media
{
    public class MediaState
    {
        public MediaStateEnum State { get; set; }
        public string Message { get; set; }
    }
}