using FluentCloudMusic.DataModels.JSONModels.Responses;
using System;

namespace FluentCloudMusic.Classes
{
    public sealed class ResponseCodeErrorException : Exception
    {
        public int Code { get; }

        public ResponseCodeErrorException(BaseResponse response) : base(response.Message)
        {
            Code = response.Code;
        }
    }

    public sealed class NoCopyrightException : Exception { }
}
