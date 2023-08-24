namespace FluentCloudMusic.DataModels.JSONModels.Responses
{

    public class LoginCellphoneResponse
    {
        public int Code { get; set; }

        public int LoginType { get; set; }

        public Profile Profile { get; set; }
    }
}
