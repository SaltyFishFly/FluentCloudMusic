using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentNetease.Classes
{
    public class NeteaseMusic : AbstractMusic
    {
        public override async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var (IsSuccess, Result) = await Network.GetOfficialMusicUrlAsync(ID);
            return IsSuccess ? Result : null;
        }
    }
}
