namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class AlbumResponse
    {
        public int Code { get; set; }

        public Song[] Songs { get; set; }

        public Album Album { get; set; }
    }
}