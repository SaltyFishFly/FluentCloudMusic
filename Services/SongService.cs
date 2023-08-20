using FluentCloudMusic.DataModels;
using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace FluentCloudMusic.Services
{
    public static class SongService
    {
        public static async Task<(bool IsSuccess, MediaPlaybackItem Result)> GetNeteaseSongUrl(Song song)
        {
            var parameters = new Dictionary<string, object> { { "id", song.ID } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.SongUrl, parameters);
            var code = jsonResult["code"].Value<int>();

            if (code != 200 || jsonResult["data"].First["url"].ToString() == string.Empty) return (false, null);

            var result = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(jsonResult["data"].First["url"].ToString())));
            return (true, result);
        }
    }
}
