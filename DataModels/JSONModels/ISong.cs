using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public interface ISong
    {
        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string ArtistName { get; }

        public string AlbumName { get; }

        public bool HasCopyright { get; }

        public Task<MediaPlaybackItem> ToMediaPlaybackItem();
    }
}
