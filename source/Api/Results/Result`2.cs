using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Company.Product.WebApi.Api.Results;

public abstract class Result<TContext, TResult> : IResult<TContext>
    where TContext : class
    where TResult : Result<TContext, TResult>
{
    protected Result(int? statusCode = null, IEnumerable<Func<ContextWrapper<TContext>, Task>>? formatters = null)
    {
        if (typeof(TContext) != typeof(ActionContext) && typeof(TContext) != typeof(HttpContext))
        {
            ThrowArgumentOutOfRangeException(
                $"Context type may only be {nameof(ActionContext)} or {nameof(HttpContext)}.",
                typeof(TContext),
                nameof(TContext));
        }

        StatusCode = statusCode;
        if (formatters is not null)
        {
            Formatters.AddRange(formatters);
        }
    }

    [JsonIgnore]
    public List<Func<ContextWrapper<TContext>, Task>> Formatters { get; } = new();

    [JsonIgnore]
    public int? StatusCode { get; private set; }

    public abstract Task ExecuteResultAsync(ContextWrapper<TContext> context);

    public TResult WithStatusCode(int statusCode)
    {
        StatusCode = statusCode;

        return (TResult)this;
    }

    public TResult AddFormatter(Func<ContextWrapper<TContext>, Task> formatter)
    {
        ThrowIfNull(formatter, nameof(formatter));

        Formatters.Add(formatter);

        return (TResult)this;
    }

    // 100 Continue

    public static TResult Continue() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status100Continue);

    // 200 OK

    public static TResult Ok() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status200OK);

    // 201 Created

    public static TResult Created(string location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status201Created)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult Created(Uri location) =>
        Created(location.ToString());

    // 202 Accepted

    public static TResult Accepted(string? location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status202Accepted)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult Accepted(Uri? location) =>
        Accepted(location?.ToString());

    // 204 No Content

    public static TResult NoContent(Action<ActionContext>? formatter) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status204NoContent);

    // 300 Multiple Choices

    public static TResult MultipleChoices(string? location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status300MultipleChoices)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult MultipleChoices(Uri? location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status300MultipleChoices)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location?.ToString();

                    return Task.CompletedTask;
                });

    // 301 Moved Permanently

    public static TResult MovedPermanently(string location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status301MovedPermanently)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult MovedPermanently(Uri location) =>
        MovedPermanently(location.ToString());

    // 302 Found

    public static TResult Found(string location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status302Found)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult Found(Uri location) =>
        Found(location.ToString());

    // 303 See Other

    public static TResult SeeOther(string location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status302Found)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult SeeOther(Uri location) =>
        SeeOther(location.ToString());

    // 304 Not Modified

    public static TResult NotModified() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status304NotModified);

    // 307 Temporary Redirect

    public static TResult TemporaryRedirect(string location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status307TemporaryRedirect)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult TemporaryRedirect(Uri location) =>
        TemporaryRedirect(location.ToString());

    // 308 Permanent Redirect

    public static TResult PermanentRedirect(string location) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status308PermanentRedirect)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.Location = location;

                    return Task.CompletedTask;
                });

    public static TResult PermanentRedirect(Uri location) =>
        PermanentRedirect(location.ToString());

    // 400 Bad Request

    public static TResult BadRequest() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status400BadRequest);

    // 401 Unauthorized

    public static TResult Unauthorized(StringValues wwwAuthenticate) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status401Unauthorized)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.WWWAuthenticate = wwwAuthenticate;

                    return Task.CompletedTask;
                });

    // 403 Forbidden

    public static TResult Forbidden() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status403Forbidden);

    // 404 Not Found

    public static TResult NotFound() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status404NotFound);

    // 405 Method Not Allowed

    public static TResult MethodNotAllowed() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status405MethodNotAllowed);

    // 406 Not Acceptable

    public static TResult NotAcceptable() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status406NotAcceptable);

    // 407 Proxy Authentication Required

    public static TResult ProxyAuthenticationRequired(StringValues proxyAuthenticate) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status407ProxyAuthenticationRequired)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.ProxyAuthenticate = proxyAuthenticate;

                    return Task.CompletedTask;
                });

    // 408 Request Timeout

    public static TResult RequestTimeout(bool closeConnection = true) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status408RequestTimeout)
            .AddFormatter(
                context =>
                {
                    if (closeConnection)
                    {
                        context.Response.Headers.Connection = "Close";
                    }

                    return Task.CompletedTask;
                });

    // 409 Conflict

    public static TResult Conflict() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status409Conflict);

    // 410 Gone

    public static TResult Gone() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status410Gone);

    // 411 Length Required

    public static TResult LengthRequired() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status411LengthRequired);

    // 412 Preconditon Failed

    public static TResult PreconditionFailed() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status412PreconditionFailed);

    // 413 Payload Too Large

    public static TResult PayloadTooLarge() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status413PayloadTooLarge);

    // 414 URI Too Large

    public static TResult UriTooLong() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status414UriTooLong);

    // 415 Unsupported Media Type

    public static TResult UnsupportedMediaType() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status415UnsupportedMediaType);

    // 416 Range Not Satisfiable

    public static TResult RangeNotSatisfiable(StringValues contentRange) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status416RangeNotSatisfiable)
            .AddFormatter(
                context =>
                {
                    context.Response.Headers.ContentRange = contentRange;

                    return Task.CompletedTask;
                });

    // 417 Expectation Failed

    public static TResult ExpectationFailed() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status417ExpectationFailed);

    // 418 I'm a Teapot

    public static TResult ImATeapot() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status418ImATeapot);

    // 428 Precondition Required

    public static TResult PreconditionRequired() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status428PreconditionRequired);

    // 431 Request Header Fields Too Large

    public static TResult RequestHeaderFieldsTooLarge() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status431RequestHeaderFieldsTooLarge);

    // 451 Unavailable for Legal Reasons

    public static TResult UnavailableForLegalReasons() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status451UnavailableForLegalReasons);

    // 500 Internal Server Error

    public static TResult InternalServerError() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status500InternalServerError);

    // 501 Not Implemented

    public static TResult NotImplemented(StringValues? retryAfter) =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status501NotImplemented)
            .AddFormatter(
                context =>
                {
                    if (retryAfter is not null)
                    {
                        context.Response.Headers.RetryAfter = retryAfter.Value;
                    }

                    return Task.CompletedTask;
                });

    // 502 Bad Gateway

    public static TResult BadGateway() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status502BadGateway);

    // 503 Service Unavailable

    public static TResult ServiceUnavailable() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status503ServiceUnavailable);

    // 504 Gateway Timeout

    public static TResult GatewayTimeout() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status504GatewayTimeout);

    // 505 HTTP Version Not Supported

    public static TResult HttpVersionNotSupported() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status505HttpVersionNotsupported);

    // 506 Variant Also Negotiates

    public static TResult VariantAlsoNegotiates() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status506VariantAlsoNegotiates);

    // 510 Not Extended

    public static TResult NotExtended() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status510NotExtended);

    // 511 Network Authentication Required

    public static TResult NetworkAuthenticationRequired() =>
        ResultFactory.Create<TContext, TResult>(StatusCodes.Status511NetworkAuthenticationRequired);
}
