using FluentCloudMusic.Utils;
using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Artist
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Alias { get; set; }

        [JsonProperty("briefDesc")]
        public string Description { get; set; }

        [MultipleJsonProperty("transNames", "trans", "tns")]
        public string[] Translations { get; set; }

        [MultipleJsonProperty("picUrl", "avatar")]
        public string ImageUrl { get; set; }
    }
}
