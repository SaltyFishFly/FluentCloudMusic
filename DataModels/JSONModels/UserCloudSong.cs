using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class UserCloudSong : ISong
    {
        [JsonIgnore]
        public UserCloudSong This { get; }

        [JsonProperty("songId")]
        public string Id { get; set; }

        [JsonProperty("songName")]
        public string Name { get; set; }

        [JsonProperty("artist")]
        public string ArtistName { get; set; }

        [JsonProperty("album")]
        public string AlbumName { get; set; }

        [JsonProperty("simpleSong")]
        public UserCloudSongData Data { get; set; }

        public string FileName { get; set; }


        public bool HasCopyright => true;

        public string ImageUrl => Data.Album.ImageUrl ?? "ms-appx:///Assets/LargeTile.scale-400.png";

        public string Description => $"( {FileName} )";

        public UserCloudSong()
        {
            This = this;
        }

        public bool RelateTo(string filter)
        {
            bool predicate(string s) => s.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
            return
                predicate(Name) ||
                predicate(Description) ||
                predicate(ArtistName) ||
                predicate(AlbumName);
        }

        public async Task<MediaPlaybackItem> ToMediaPlaybackItem()
        {
            var (isSuccess, result) = await SongService.GetNeteaseSongUrl(this);
            return isSuccess ? result : null;
        }

        public class UserCloudSongData
        {
            [JsonProperty("al")]
            public Album Album { get; set; }
        }
    }
}
