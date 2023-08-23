using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Profile
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
    }

}
