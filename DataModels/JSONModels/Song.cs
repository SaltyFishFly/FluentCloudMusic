using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels
{
    public class Song
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Alias { get; set; }

        [JsonProperty("ar")]
        public Ar[] Artists { get; set; }

        [JsonProperty("al")]
        public Al Album { get; set; }

        [JsonProperty("noCopyrightRcmd")]
        public object NoCopyrightRecommendation { get; set; }

        [JsonProperty("hr")]
        public QualityInfo HiResQuality { get; set; }

        [JsonProperty("sq")]
        public QualityInfo SuperQuality { get; set; }

        [JsonProperty("h")]
        public QualityInfo HighQuality { get; set; }

        [JsonProperty("m")]
        public QualityInfo MediumQuality { get; set; }

        [JsonProperty("l")]
        public QualityInfo LowQuality { get; set; }

        [JsonProperty("tns")]
        public string[] Translations { get; set; }
    }

    public class Ar
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("tns")]
        public string[] Translations { get; set; }

        public string[] Alias { get; set; }
    }

    public class Al
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("picUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("alia")]
        public string[] Alias { get; set; }
    }

    public class QualityInfo
    {
        [JsonProperty("br")]
        public int BitRate { get; set; }

        public int Fid { get; set; }

        public int Size { get; set; }

        public float Vd { get; set; }

        public int Sr { get; set; }
    }

}
