using Newtonsoft.Json;

namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class SongUrlV1Response : BaseResponse
    {
        public SongUrlV1ResponseData[] Data { get; set; }
    }

    public class SongUrlV1ResponseData
    {
        public int Code { get; set; }

        public int Id { get; set; }

        public int Time { get; set; }

        public string Url { get; set; }

        [JsonProperty("level")]
        public string Quality { get; set; }

        [JsonProperty("br")]
        public int BitRate { get; set; }

        public int Size { get; set; }

        public string Md5 { get; set; }

        public string Type { get; set; }

        public string EncodeType { get; set; }
    }
}
