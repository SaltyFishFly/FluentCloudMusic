using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Profile
    {
        [JsonProperty("userId")]
        public string UserID { get; set; }

        [JsonProperty("userType")]
        public int UserType { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("followed")]
        public bool Followed { get; set; }

        [JsonProperty("followeds")]
        public int FollowerCount { get; set; }

        [JsonProperty("follows")]
        public int FollowingCount { get; set; }

        [JsonProperty("vipType")]
        public int VipType { get; set; }
    }

}
