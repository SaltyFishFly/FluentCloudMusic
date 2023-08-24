namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class UserCloudResponse
    {
        public int Code { get; set; }

        public int Count { get; set; }

        public UserCloudSong[] Data { get; set; }
    }
}
