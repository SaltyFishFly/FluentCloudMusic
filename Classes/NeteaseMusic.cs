using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.Classes
{
    public class NeteaseMusic : AbstractMusic
    {
        public override async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var (isSuccess, result) = await Network.GetOfficialMusicUrlAsync(ID);
            return isSuccess ? result : null;
        }
    }
}
