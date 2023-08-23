using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class LoginStatusResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("Profile")]
        public Profile Profile { get; set; }
    }
}
