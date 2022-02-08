using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace MindKeeper.Shared.Wrappers
{
    public class Response<T> : Response
    {
        public Response()
        {
        }
        public Response(T data, string message = null)
        {
            Succeeded = true;
            Status = (int)HttpStatusCode.OK;
            Message = message;
            Data = data;
        }
        public Response(string message)
        {
            Succeeded = false;
            Status = (int)HttpStatusCode.BadRequest;
            Message = message;
        }
        //public bool Succeeded { get; set; }
        //public string Message { get; set; }
        //public List<string> Errors { get; set; }
        public T Data { get; set; }
    }

    public class Response
    {
        public Response()
        {
            Succeeded = true;
            Status = (int)HttpStatusCode.OK;
        }
        
        public Response(string message)
        {
            Succeeded = false;
            Status = (int)HttpStatusCode.BadRequest;
            Message = message;
        }

        public int Status { get; set; }
        public bool Succeeded { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; }
    }
}
