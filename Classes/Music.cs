using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentNetease.Classes
{
    public class Music
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var (IsSuccess, Result) = await Network.GetMusicUrlAsync(ID);
            return IsSuccess ? Result : null;
        }
    }
}
