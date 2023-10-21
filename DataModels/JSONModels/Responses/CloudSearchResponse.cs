namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class CloudSearchResponse : BaseResponse
    {
        public CloudSearchResponseResult Result { get; set; }

        public class CloudSearchResponseResult
        {
            public Song[] Songs { get; set; }

            public int SongCount { get; set; }
        }
    }
}
