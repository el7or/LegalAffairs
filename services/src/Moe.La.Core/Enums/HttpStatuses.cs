namespace Moe.La.Core.Enums
{
    /// <summary>
    /// Provides the required status codes for the HTTP responses.
    /// </summary>
    public enum HttpStatuses
    {
        Status200OK = 200,
        Status201Created = 201,
        Status400BadRequest = 400,
        Status401Unauthorized = 401,
        Status404NotFound = 404,
        Status500InternalServerError = 500,
        Status503ServiceUnavailable = 503
    }
}
