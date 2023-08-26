using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentCloudMusic.Services
{
    public static class PlaylistService
    {
        public static async Task<List<Playlist>> GetDailyRecommendPlaylistsAsync()
        {
            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.RecommendResource);
            var result = jsonResult.ToObject<RecommendResourcesResponse>(JsonUtils.Serializer);
            return result.Code == 200 ? result.Playlists.ToList() : new List<Playlist>();
        }

        public static async Task<(bool IsSuccess, List<Playlist> Playlists)> GetUserPlaylist(string uid)
        {
            var parameters = new Dictionary<string, object> { { "uid", uid } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserPlaylist, parameters);
            var result = jsonResult.ToObject<UserPlaylistResponse>(JsonUtils.Serializer);

            return result.Code == 200 ? (true, result.Playlists.ToList()) : (false, null);
        }

        public static async Task<(bool IsSuccess, Playlist Info, List<Song> Songs)> GetPlaylistDetailAsync(string playlistID)
        {
            var playlistParams = new Dictionary<string, object> { { "id", playlistID } };

            var jsonResult1 = await App.API.RequestAsync(CloudMusicApiProviders.PlaylistDetail, playlistParams);
            var result1 = jsonResult1.ToObject<PlaylistDetailResponse>(JsonUtils.Serializer);

            if (result1.Code != 200) return (false, null, null);
            if (result1.Playlist.TrackIds.Length == 0) return (true, result1.Playlist, new List<Song>());


            StringBuilder musicIDsBuilder = new StringBuilder();
            foreach (var track in result1.Playlist.TrackIds) musicIDsBuilder.Append(track.Id).Append(",");
            musicIDsBuilder.Remove(musicIDsBuilder.Length - 1, 1);

            var songParams = new Dictionary<string, object> { { "ids", musicIDsBuilder.ToString() } };
            var jsonResult2 = await App.API.RequestAsync(CloudMusicApiProviders.SongDetail, songParams);
            var result2 = jsonResult2.ToObject<SongDetailResponse>(JsonUtils.Serializer);

            if (result2.Code != 200) return (true, result1.Playlist, new List<Song>());

            return (true, result1.Playlist, result2.Songs.ToList());
        }
    }
}
