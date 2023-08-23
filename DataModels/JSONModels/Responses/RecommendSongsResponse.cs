namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class RecommendSongsResponse
    {
        public int Code { get; set; }

        public RecommendSongsResponseData Data { get; set; }
    }

    public class RecommendSongsResponseData
    {
        public Song[] DailySongs { get; set; }
    }
}
