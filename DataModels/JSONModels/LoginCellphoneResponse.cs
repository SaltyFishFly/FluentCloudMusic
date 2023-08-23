using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{

    public class LoginCellphoneResponse
    {
        [JsonProperty("loginType")]
        public int LoginType { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}
