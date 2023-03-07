using System.Net;

namespace Go.Models
{
    public class ErrorData
    {
        public string Message { get; set; }
    }

    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public ErrorData ErrorData { get; set; }
    }

    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public ErrorData ErrorData { get; set; }
        public T Data { get; set; }
    }
}