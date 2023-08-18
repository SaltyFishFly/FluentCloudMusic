using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.DataModels
{
    public abstract class AbstractMusic
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        abstract public Task<MediaPlaybackItem> ToMediaPlaybackItem();
    }
}
