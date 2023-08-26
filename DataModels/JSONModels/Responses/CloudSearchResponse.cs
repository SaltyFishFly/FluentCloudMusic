namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class CloudSearchResponse
    {
        public int Code { get; set; }

        public CloudSearchResponseResult Result { get; set; }

        public class CloudSearchResponseResult
        {
            public Song[] Songs { get; set; }

            public int SongCount { get; set; }
        }
    }
}
