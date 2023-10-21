namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class AlbumResponse : BaseResponse
    {
        public Album Album { get; set; }

        public Song[] Songs { get; set; }
    }
}