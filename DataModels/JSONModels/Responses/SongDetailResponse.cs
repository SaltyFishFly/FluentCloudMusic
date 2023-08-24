namespace FluentCloudMusic.DataModels.JSONModels.Responses
{

    public class SongDetailResponse
    {
        public int Code { get; set; }

        public Song[] Songs { get; set; }
    }
}
