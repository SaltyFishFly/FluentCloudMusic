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
        public static async Task<(bool IsSuccess, MediaPlaybackItem Result)> GetNeteaseSongUrl(DeprecatedSong song)
        {
            if (song == null || !song.HasCopyright) return (false, null);

            var parameters = new Dictionary<string, object> { { "id", song.ID } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.SongUrl, parameters);
            var code = jsonResult["code"].Value<int>();

            if (code != 200 || jsonResult["data"].First["url"].ToString() == string.Empty) return (false, null);

            var result = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(jsonResult["data"].First["url"].ToString())));

            var metadata = result.GetDisplayProperties();
            metadata.Type = Windows.Media.MediaPlaybackType.Music;
            metadata.MusicProperties.Title = $"{song.Name}{song.Alias}";
            metadata.MusicProperties.Artist = song.Artists.MainArtist.Name;
            metadata.MusicProperties.AlbumTitle = song.Album.Name;
            result.ApplyDisplayProperties(metadata);

            return (true, result);
        }
    }
}
