namespace SmartHint.API.Extensions;

public static class HttpExtensions
{
    public static bool IsSuccess(this HttpStatusCode statusCode) => new HttpResponseMessage(statusCode).IsSuccessStatusCode;
}
