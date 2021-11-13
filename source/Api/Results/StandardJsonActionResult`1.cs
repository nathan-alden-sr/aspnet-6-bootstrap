using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Results;

public class StandardJsonActionResult<TData> : StandardJsonActionResult, IStandardJsonResult<ActionContext, TData>
{
    public StandardJsonActionResult(
        int? statusCode = null,
        TData? data = default,
        string? message = null,
        IEnumerable<Func<ContextWrapper<ActionContext>, Task>>? formatters = null)
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

    public StandardJsonActionResult<TData> WithData(TData? data)
    {
        Data = data;

        return this;
    }
}
