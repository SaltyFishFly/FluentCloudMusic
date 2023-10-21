using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class UserPlaylistResponse : BaseResponse
    {
        [JsonProperty("playlist")]
        public Playlist[] Playlists { get; set; }
    }
}
