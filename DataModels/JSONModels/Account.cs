using FluentCloudMusic.DataModels.JSONModels.Responses;
using NeteaseCloudMusicApi;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Account
    {
        public string UserId { get; set; }

        public int UserType { get; set; }

        public int VipType { get; set; }

        public string Nickname { get; set; }

        public string AvatarUrl { get; set; }

        public string Signature { get; set; }

        public bool Followed { get; set; }

        [JsonProperty("followeds")]
        public int FollowerCount { get; set; }

        [JsonProperty("follows")]
        public int FollowingCount { get; set; }

        public async Task<List<Playlist>> GetPlaylistsAsync()
        {
            var parameters = new Dictionary<string, object> { { "uid", UserId } };

            var jsonResult = await App.API.RequestAsync(CloudMusicApiProviders.UserPlaylist, parameters);
            var result = jsonResult.ToObject<UserPlaylistResponse>();
            result.CheckCode();

            return result.Playlists.ToList();
        }
    }
}
