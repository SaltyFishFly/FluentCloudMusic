namespace FluentCloudMusic.DataModels.JSONModels.Responses
{

    public class ArtistDetailResponse : BaseResponse
    {
        public ArtistDetailResponseData Data { get; set; }

        public class ArtistDetailResponseData
        {
            public Artist Artist { get; set; }
        }
    }
}
