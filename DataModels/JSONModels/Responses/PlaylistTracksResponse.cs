namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class PlaylistTracksResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string TrackIds { get; set; }
        public int Count { get; set; }
        public int CloudCount { get; set; }
    }
}
