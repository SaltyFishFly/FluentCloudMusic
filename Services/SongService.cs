using FluentCloudMusic.Classes;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace FluentCloudMusic.Services
{
    public static class SongService
    {
        public static async Task<List<Song>> GetDailyRecommendSongsAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.RecommendSongs);
            var result = jsonResult.ToObject<RecommendSongsResponse>();
            result.CheckCode();

            return result.Data.DailySongs.ToList();
        }

        public static async Task<MediaPlaybackItem> GetNeteaseSongUrl(ISong song)
        {
            if (song == null) throw new ArgumentNullException();
            if (!song.HasCopyright) throw new NoCopyrightException();

            var parameters = new Dictionary<string, object> { { "id", song.Id }, { "level", "standard" } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.SongUrlV1, parameters);
            var result = jsonResult.ToObject<SongUrlV1Response>();
            result.CheckCode();

            var item = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(result.Data.First().Url)));
            item.SetMetadata(song);

            return item;
        }
    }
}
