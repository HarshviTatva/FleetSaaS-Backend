namespace FleetSaaS.Infrastructure.Common.Response
{
    public abstract class ApiResponse
    {
        public bool Result { get; set; }

        public int HttpStatusCode { get; set; }

        public IReadOnlyList<string> Messages { get; set; } = new List<string>();
    }

    public class SuccessApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public SuccessApiResponse(int httpStatusCode, List<string> message, T? data)
        {
            Result = true;
            HttpStatusCode = httpStatusCode;
            Messages = message;
            Data = data;
        }
    }

    public class ErrorApiResponse : ApiResponse
    {
        public IReadOnlyDictionary<string, object>? Metadata { get; set; }
    }
}
