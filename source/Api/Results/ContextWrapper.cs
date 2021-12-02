using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Results;

public sealed class ContextWrapper<T>
    where T : class
{
    private readonly HttpContext _httpContext;

    public ContextWrapper(T context)
    {
        ThrowIfNull(context);

        if (typeof(T) == typeof(ActionContext))
        {
            _httpContext = ((ActionContext)(object)context).HttpContext;
        }
        else if (typeof(T) == typeof(HttpContext))
        {
            _httpContext = (HttpContext)(object)context;
        }
        else
        {
            ThrowArgumentOutOfRangeException(
                $"Context type may only be {nameof(ActionContext)} or {nameof(HttpContext)}.",
                typeof(T),
                nameof(context));
        }

        Context = context;
    }

    public T Context { get; }
    public IFeatureCollection Features => _httpContext.Features;
    public HttpRequest Request => _httpContext.Request;
    public HttpResponse Response => _httpContext.Response;
    public ConnectionInfo Connection => _httpContext.Connection;
    public WebSocketManager WebSockets => _httpContext.WebSockets;

    public ClaimsPrincipal User
    {
        get => _httpContext.User;
        set => _httpContext.User = value;
    }

    public IDictionary<object, object?> Items
    {
        get => _httpContext.Items;
        set => _httpContext.Items = value;
    }

    public IServiceProvider RequestServices
    {
        get => _httpContext.RequestServices;
        set => _httpContext.RequestServices = value;
    }

    public CancellationToken RequestAborted
    {
        get => _httpContext.RequestAborted;
        set => _httpContext.RequestAborted = value;
    }

    public string TraceIdentifier
    {
        get => _httpContext.TraceIdentifier;
        set => _httpContext.TraceIdentifier = value;
    }

    public ISession Session
    {
        get => _httpContext.Session;
        set => _httpContext.Session = value;
    }
}
