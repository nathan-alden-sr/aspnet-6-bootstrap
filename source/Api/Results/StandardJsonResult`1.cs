using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Results;

public sealed class StandardJsonResult<TData> : StandardJsonResult, IStandardJsonResult<TData>
{
    public StandardJsonResult(int? statusCode = null, TData? data = default, string? message = null, IEnumerable<Action<ActionContext>>? formatters = null)
        : base(statusCode, message, formatters)
    {
        Data = data;
    }

    public TData? Data { get; private set; }

    protected override object GetStandardJsonAnonymousObject() =>
        new
        {
            Data,
            Message
        };

    public StandardJsonResult<TData> WithData(TData? data)
    {
        Data = data;

        return this;
    }
}