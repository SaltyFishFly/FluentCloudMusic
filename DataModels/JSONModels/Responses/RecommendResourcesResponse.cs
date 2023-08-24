using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels.Responses
{

    public class RecommendResourcesResponse
    {
        public int Code { get; set; }

        [JsonProperty("recommend")]
        public Playlist[] Playlists { get; set; }
    }
}
