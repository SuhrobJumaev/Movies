using System.Net;

namespace Movies.Web;

public class ValidationFailureResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public string Message  {get; init;}
    public required IEnumerable<ValidationResponse> Errors { get; init; }
}

public class ValidationResponse
{
    public required string PropertyName { get; init; }
    public required string Message { get; init; }
}
