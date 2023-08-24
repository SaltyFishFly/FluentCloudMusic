using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
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
        public static async Task<(bool IsSuccess, MediaPlaybackItem Result)> GetNeteaseSongUrl(ISong song)
        {
            if (song == null || !song.HasCopyright) return (false, null);

            var parameters = new Dictionary<string, object> { { "id", song.Id } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.SongUrl, parameters);
            var code = jsonResult["code"].Value<int>();

            if (code != 200 || jsonResult["data"].First["url"].ToString() == string.Empty) return (false, null);

            var result = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(jsonResult["data"].First["url"].ToString())));

            var metadata = result.GetDisplayProperties();
            metadata.Type = Windows.Media.MediaPlaybackType.Music;
            metadata.MusicProperties.Title = $"{song.Name}{song.Description}";
            metadata.MusicProperties.Artist = song.ArtistName;
            metadata.MusicProperties.AlbumTitle = song.AlbumName;
            result.ApplyDisplayProperties(metadata);

            return (true, result);
        }
    }
}
