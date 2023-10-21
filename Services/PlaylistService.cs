using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
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
            var result = jsonResult.ToObject<RecommendResourcesResponse>();
            result.CheckCode();

            return result.Playlists.ToList();
        }

        public static async Task<(Playlist Info, List<Song> Songs)> GetPlaylistDetailAsync(string playlistID)
        {
            var playlistParams = new Dictionary<string, object> { { "id", playlistID } };

            var jsonResult1 = await App.API.RequestAsync(CloudMusicApiProviders.PlaylistDetail, playlistParams);
            var result1 = jsonResult1.ToObject<PlaylistDetailResponse>();
            result1.CheckCode();

            if (result1.Playlist.TrackIds.Length == 0) return (result1.Playlist, new List<Song>());

            StringBuilder musicIDsBuilder = new StringBuilder();
            foreach (var track in result1.Playlist.TrackIds) musicIDsBuilder.Append(track.Id).Append(",");
            musicIDsBuilder.Remove(musicIDsBuilder.Length - 1, 1);

            var songParams = new Dictionary<string, object> { { "ids", musicIDsBuilder.ToString() } };
            var jsonResult2 = await App.API.RequestAsync(CloudMusicApiProviders.SongDetail, songParams);
            var result2 = jsonResult2.ToObject<SongDetailResponse>();
            result2.CheckCode();
            
            return (result1.Playlist, result2.Songs.ToList());
        }
    }
}
