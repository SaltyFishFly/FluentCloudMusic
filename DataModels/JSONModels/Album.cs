using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Album
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string[] Alias { get; set; }

        [JsonProperty("picUrl")]
        public string ImageUrl { get; set; }

        public Artist[] Artists { get; set; }

        public long PublishTime { get; set; }
    }
}
