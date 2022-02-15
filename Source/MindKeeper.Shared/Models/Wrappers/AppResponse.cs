using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace MindKeeper.Shared.Wrappers
{
    public class AppResponse<T>
    {
        protected AppResponse()
        {
        }

        public static AppResponse<T> Ok(T data, string message = null)
        {
            return new AppResponse<T>
            {
                Succeeded = true,
                Status = (int)HttpStatusCode.OK,
                Message = message,
                Data = data
            };
        }

        public static AppResponse<T> Error(string message)
        {
            return new AppResponse<T>
            {
                Succeeded = false,
                Status = (int)HttpStatusCode.BadRequest,
                Message = message
            };
        }

        public int Status { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public T Data { get; set; }
    }

    public class AppResponse
    {
        protected AppResponse()
        {
        }

        public static AppResponse Ok(string message = null)
        {
            return new AppResponse
            {
                Succeeded = true,
                Status = (int)HttpStatusCode.OK,
                Message = message
            };
        }

        public static AppResponse Error(string message)
        {
            return new AppResponse
            {
                Succeeded = false,
                Status = (int)HttpStatusCode.BadRequest,
                Message = message
            };
        }

        public int Status { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
