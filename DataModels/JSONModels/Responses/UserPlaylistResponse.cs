using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class UserPlaylistResponse
    {
        public int Code { get; set; }

        [JsonProperty("playlist")]
        public Playlist[] Playlists { get; set; }
    }
}
