namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class RecommendSongsResponse : BaseResponse
    {
        public RecommendSongsResponseData Data { get; set; }
    }

    public class RecommendSongsResponseData
    {
        public Song[] DailySongs { get; set; }
    }
}
