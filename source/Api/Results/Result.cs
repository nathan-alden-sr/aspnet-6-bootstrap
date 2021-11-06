// Portions of this source code (c) the .NET Foundation.

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;
using Company.Product.WebApi.Api.Filters.Validation;

namespace Company.Product.WebApi.Api.Results;

public class Result : IResult
{
    public Result(int? statusCode = null, IEnumerable<Action<ActionContext>>? formatters = null)
    {
        StatusCode = statusCode;
        if (formatters is not null)
        {
            Formatters.AddRange(formatters);
        }
    }

    [JsonIgnore]
    public List<Action<ActionContext>> Formatters { get; } = new();

    [JsonIgnore]
    IReadOnlyCollection<Action<ActionContext>> IResult.Formatters => Formatters;

    [JsonIgnore]
    public int? StatusCode { get; private set; }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        ThrowIfNull(context, nameof(context));

        if (StatusCode is null)
        {
            ThrowInvalidOperationException("Status code is required.");
        }

        context.HttpContext.Response.StatusCode = StatusCode.Value;

        foreach (Action<ActionContext> formatter in Formatters)
        {
            formatter(context);
        }

        await ExecuteResultInternalAsync(context);
    }

    public Result WithStatusCode(int statusCode)
    {
        StatusCode = statusCode;

        return this;
    }

    public Result WithFormatting(Action<ActionContext> formatter)
    {
        ThrowIfNull(formatter, nameof(formatter));

        Formatters.Add(formatter);

        return this;
    }

    public StandardJsonResult AsStandardJson() =>
        new(StatusCode, null, Formatters);

    public StandardJsonResult<TData> AsStandardJson<TData>(TData? data) =>
        new(StatusCode, data, null, Formatters);

    public StandardJsonResult<TData> AsStandardJson<TData>(TData? data, string? message) =>
        new(StatusCode, data, message, Formatters);

    // 100 Continue

    public static Result Continue() => new(StatusCodes.Status100Continue);

    // 200 OK

    public static Result Ok() => new(StatusCodes.Status200OK);

    // 201 Created

    public static Result Created(string location) =>
        new Result(StatusCodes.Status201Created).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result Created(Uri location) =>
        Created(location.ToString());

    public static Result CreatedAtAction(string? actionName, string? controllerName, object? routeValues) =>
        new Result(StatusCodes.Status201Created)
            .WithFormatting(
                context =>
                {
                    HttpRequest request = context.HttpContext.Request;
                    IUrlHelper urlHelper = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context);
                    string? url = urlHelper.Action(actionName, controllerName, routeValues, request.Scheme, request.Host.ToUriComponent());

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.HttpContext.Response.Headers.Location = url;
                });

    public static Result CreatedAtAction(string? actionName, object? routeValues) =>
        CreatedAtAction(actionName, null, routeValues);

    public static Result CreatedAtAction(string? actionName) =>
        CreatedAtAction(actionName, null, null);

    public static Result CreatedAtRoute(string? routeName, object? routeValues) =>
        new Result(StatusCodes.Status201Created)
            .WithFormatting(
                context =>
                {
                    ThrowIfNull(context, nameof(context));

                    IUrlHelper urlHelper = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context);
                    string? url = urlHelper.Link(routeName, routeValues);

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.HttpContext.Response.Headers.Location = url;
                });

    public static Result CreatedAtRoute(string? routeName) =>
        CreatedAtRoute(routeName, null);

    public static Result CreatedAtRoute(object? routeValues) =>
        CreatedAtRoute(null, routeValues);

    // 202 Accepted

    public static Result Accepted(string? location) =>
        new Result(StatusCodes.Status202Accepted).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result Accepted(Uri? location) =>
        Accepted(location?.ToString());

    public static Result AcceptedAtAction(string? actionName, string? controllerName, object? routeValues) =>
        new Result(StatusCodes.Status202Accepted)
            .WithFormatting(
                context =>
                {
                    HttpRequest request = context.HttpContext.Request;
                    IUrlHelper urlHelper = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context);
                    string? url = urlHelper.Action(actionName, controllerName, routeValues, request.Scheme, request.Host.ToUriComponent());

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.HttpContext.Response.Headers.Location = url;
                });

    public static Result AcceptedAtAction(string? actionName, object? routeValues) =>
        AcceptedAtAction(actionName, null, routeValues);

    public static Result AcceptedAtAction(string? actionName) =>
        AcceptedAtAction(actionName, null, null);

    public static Result AcceptedAtRoute(string? routeName, object? routeValues) =>
        new Result(StatusCodes.Status202Accepted)
            .WithFormatting(
                context =>
                {
                    ThrowIfNull(context, nameof(context));

                    IUrlHelper urlHelper = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context);
                    string? url = urlHelper.Link(routeName, routeValues);

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.HttpContext.Response.Headers.Location = url;
                });

    public static Result AcceptedAtRoute(string? routeName) =>
        AcceptedAtRoute(routeName, null);

    public static Result AcceptedAtRoute(object? routeValues) =>
        AcceptedAtRoute(null, routeValues);

    // 204 No Content

    public static NoContentResponse NoContent(Action<ActionContext>? formatter) =>
        new(formatter);

    // 300 Multiple Choices

    public static Result MultipleChoices(string? location) =>
        new Result(StatusCodes.Status300MultipleChoices).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result MultipleChoices(Uri? location) =>
        new Result(StatusCodes.Status300MultipleChoices).WithFormatting(context => context.HttpContext.Response.Headers.Location = location?.ToString());

    public static Result MultipleChoicesWithPreferredAction(string? actionName, string? controllerName, object? routeValues) =>
        new Result(StatusCodes.Status300MultipleChoices)
            .WithFormatting(
                context =>
                {
                    HttpRequest request = context.HttpContext.Request;
                    IUrlHelper urlHelper = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context);
                    string? url = urlHelper.Action(actionName, controllerName, routeValues, request.Scheme, request.Host.ToUriComponent());

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.HttpContext.Response.Headers.Location = url;
                });

    public static Result MultipleChoicesWithPreferredAction(string? actionName, object? routeValues) =>
        MultipleChoicesWithPreferredAction(actionName, null, routeValues);

    public static Result MultipleChoicesWithPreferredAction(string? actionName) =>
        MultipleChoicesWithPreferredAction(actionName, null, null);

    public static Result MultipleChoicesWithPreferredRoute(string? routeName, object? routeValues) =>
        new Result(StatusCodes.Status300MultipleChoices)
            .WithFormatting(
                context =>
                {
                    ThrowIfNull(context, nameof(context));

                    IUrlHelper urlHelper = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context);
                    string? url = urlHelper.Link(routeName, routeValues);

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.HttpContext.Response.Headers.Location = url;
                });

    public static Result MultipleChoicesWithPreferredRoute(string? routeName) =>
        MultipleChoicesWithPreferredRoute(routeName, null);

    public static Result MultipleChoicesWithPreferredRoute(object? routeValues) =>
        MultipleChoicesWithPreferredRoute(null, routeValues);

    // 301 Moved Permanently

    public static Result MovedPermanently(string location) =>
        new Result(StatusCodes.Status301MovedPermanently).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result MovedPermanently(Uri location) =>
        MovedPermanently(location.ToString());

    // 302 Found

    public static Result Found(string location) =>
        new Result(StatusCodes.Status302Found).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result Found(Uri location) =>
        Found(location.ToString());

    // 303 See Other

    public static Result SeeOther(string location) =>
        new Result(StatusCodes.Status302Found).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result SeeOther(Uri location) =>
        SeeOther(location.ToString());

    // 304 Not Modified

    public static Result NotModified() => new(StatusCodes.Status304NotModified);

    // 307 Temporary Redirect

    public static Result TemporaryRedirect(string location) =>
        new Result(StatusCodes.Status307TemporaryRedirect).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result TemporaryRedirect(Uri location) =>
        TemporaryRedirect(location.ToString());

    // 308 Permanent Redirect

    public static Result PermanentRedirect(string location) =>
        new Result(StatusCodes.Status308PermanentRedirect).WithFormatting(context => context.HttpContext.Response.Headers.Location = location);

    public static Result PermanentRedirect(Uri location) =>
        PermanentRedirect(location.ToString());

    // 400 Bad Request

    public static Result BadRequest() => new(StatusCodes.Status400BadRequest);

    public static StandardJsonResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        string? message,
        IEnumerable<ValidationFailureResultData>? messages) =>
        new(StatusCodes.Status400BadRequest, messages, message);

    public static StandardJsonResult<IEnumerable<ValidationFailureResultData>> BadRequest(IEnumerable<ValidationFailureResultData> messages) =>
        new(StatusCodes.Status400BadRequest, messages);

    public static StandardJsonResult<IEnumerable<ValidationFailureResultData>> BadRequest(string? message, params string[] messages) =>
        BadRequest(message, messages.Select(a => new ValidationFailureResultData(a)));

    public static StandardJsonResult<IEnumerable<ValidationFailureResultData>> BadRequest(params string[] messages) =>
        BadRequest(messages.Select(a => new ValidationFailureResultData(a)));

    // 401 Unauthorized

    public static Result Unauthorized(StringValues wwwAuthenticate) =>
        new Result(StatusCodes.Status401Unauthorized)
            .WithFormatting(context => context.HttpContext.Response.Headers.WWWAuthenticate = wwwAuthenticate);

    // 403 Forbidden

    public static Result Forbidden() => new(StatusCodes.Status403Forbidden);

    // 404 Not Found

    public static Result NotFound() => new(StatusCodes.Status404NotFound);

    // 405 Method Not Allowed

    public static Result MethodNotAllowed() => new(StatusCodes.Status405MethodNotAllowed);

    // 406 Not Acceptable

    public static Result NotAcceptable() => new(StatusCodes.Status406NotAcceptable);

    // 407 Proxy Authentication Required

    public static Result ProxyAuthenticationRequired(StringValues proxyAuthenticate) =>
        new Result(StatusCodes.Status407ProxyAuthenticationRequired)
            .WithFormatting(context => context.HttpContext.Response.Headers.ProxyAuthenticate = proxyAuthenticate);

    // 408 Request Timeout

    public static Result RequestTimeout(bool closeConnection = true) =>
        new Result(StatusCodes.Status408RequestTimeout)
            .WithFormatting(
                context =>
                {
                    if (closeConnection)
                    {
                        context.HttpContext.Response.Headers.Connection = "Close";
                    }
                });

    // 409 Conflict

    public static Result Conflict() => new(StatusCodes.Status409Conflict);

    // 410 Gone

    public static Result Gone() => new(StatusCodes.Status410Gone);

    // 411 Length Required

    public static Result LengthRequired() => new(StatusCodes.Status411LengthRequired);

    // 412 Preconditon Failed

    public static Result PreconditionFailed() => new(StatusCodes.Status412PreconditionFailed);

    // 413 Payload Too Large

    public static Result PayloadTooLarge() => new(StatusCodes.Status413PayloadTooLarge);

    // 414 URI Too Large

    public static Result UriTooLong() => new(StatusCodes.Status414UriTooLong);

    // 415 Unsupported Media Type

    public static Result UnsupportedMediaType() => new(StatusCodes.Status415UnsupportedMediaType);

    // 416 Range Not Satisfiable

    public static Result RangeNotSatisfiable(StringValues contentRange) =>
        new Result(StatusCodes.Status416RangeNotSatisfiable)
            .WithFormatting(context => context.HttpContext.Response.Headers.ContentRange = contentRange);

    // 417 Expectation Failed

    public static Result ExpectationFailed() => new(StatusCodes.Status417ExpectationFailed);

    // 418 I'm a Teapot

    public static Result ImATeapot() => new(StatusCodes.Status418ImATeapot);

    // 428 Precondition Required

    public static Result PreconditionRequired() => new(StatusCodes.Status428PreconditionRequired);

    // 431 Request Header Fields Too Large

    public static Result RequestHeaderFieldsTooLarge() => new(StatusCodes.Status431RequestHeaderFieldsTooLarge);

    // 451 Unavailable for Legal Reasons

    public static Result UnavailableForLegalReasons() => new(StatusCodes.Status451UnavailableForLegalReasons);

    // 500 Internal Server Error

    public static Result InternalServerError() => new(StatusCodes.Status500InternalServerError);

    // 501 Not Implemented

    public static Result NotImplemented(StringValues? retryAfter) =>
        new Result(StatusCodes.Status501NotImplemented)
            .WithFormatting(
                context =>
                {
                    if (retryAfter is not null)
                    {
                        context.HttpContext.Response.Headers.RetryAfter = retryAfter.Value;
                    }
                });

    // 502 Bad Gateway

    public static Result BadGateway() => new(StatusCodes.Status502BadGateway);

    // 503 Service Unavailable

    public static Result ServiceUnavailable() => new(StatusCodes.Status503ServiceUnavailable);

    // 504 Gateway Timeout

    public static Result GatewayTimeout() => new(StatusCodes.Status504GatewayTimeout);

    // 505 HTTP Version Not Supported

    public static Result HttpVersionNotSupported() => new(StatusCodes.Status505HttpVersionNotsupported);

    // 506 Variant Also Negotiates

    public static Result VariantAlsoNegotiates() => new(StatusCodes.Status506VariantAlsoNegotiates);

    // 510 Not Extended

    public static Result NotExtended() => new(StatusCodes.Status510NotExtended);

    // 511 Network Authentication Required

    public static Result NetworkAuthenticationRequired() => new(StatusCodes.Status511NetworkAuthenticationRequired);

    protected virtual Task ExecuteResultInternalAsync(ActionContext context) =>
        Task.CompletedTask;
}