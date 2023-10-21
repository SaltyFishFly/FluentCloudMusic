namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class UserCloudResponse : BaseResponse
    {
        public int Count { get; set; }

        public UserCloudSong[] Data { get; set; }
    }
}
