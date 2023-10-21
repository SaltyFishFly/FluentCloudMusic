using FluentCloudMusic.Classes;
using FluentCloudMusic.Utils;
using System;

namespace FluentCloudMusic.DataModels.JSONModels.Responses
{
    public class BaseResponse
    {
        public int Code { get; set; }

        [JsonMultipleProperty("msg", "message")]
        public string Message { get; set; }

        public void CheckCode()
        {
            if (Code != 200) throw new ResponseCodeErrorException(this);
        }

        public void CheckCode(Action errorAction)
        {
            if (Code != 200)
            {
                errorAction?.Invoke();
                throw new ResponseCodeErrorException(this);
            }
        }
    }
}
