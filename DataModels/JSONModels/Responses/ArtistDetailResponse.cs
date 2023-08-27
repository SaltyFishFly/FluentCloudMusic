namespace FluentCloudMusic.DataModels.JSONModels.Responses
{

    public class ArtistDetailResponse
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public ArtistDetailResponseData Data { get; set; }

        public class ArtistDetailResponseData
        {
            public Artist Artist { get; set; }
        }
    }
}
