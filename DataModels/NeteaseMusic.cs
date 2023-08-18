using FluentCloudMusic.Services;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.DataModels
{
    public class NeteaseMusic : AbstractMusic
    {
        public override async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var (isSuccess, result) = await NetworkService.GetOfficialMusicUrlAsync(ID);
            return isSuccess ? result : null;
        }
    }
}
