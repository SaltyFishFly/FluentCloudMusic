namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class PlaylistTracksResponse : BaseResponse
    {
        public string TrackIds { get; set; }
        public int Count { get; set; }
        public int CloudCount { get; set; }
    }
}
