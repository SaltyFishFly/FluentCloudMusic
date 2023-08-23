using FluentCloudMusic.Utils;
using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Artist
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Alias { get; set; }

        [MultipleJsonProperty("trans", "tns")]
        public string[] Translations { get; set; }

        [JsonProperty("picUrl")]
        public string ImageUrl { get; set; }
    }
}
