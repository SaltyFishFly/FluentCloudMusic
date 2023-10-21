namespace FluentCloudMusic.DataModels.JSONModels.Responses
{

    public class LoginCellphoneResponse : BaseResponse
    {
        public int LoginType { get; set; }

        public Account Profile { get; set; }
    }
}
