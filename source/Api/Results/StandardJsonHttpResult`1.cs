namespace Company.Product.WebApi.Api.Results;

public class StandardJsonHttpResult<TData> : StandardJsonHttpResult, IStandardJsonResult<HttpContext, TData>
{
    public StandardJsonHttpResult(
        int? statusCode = null,
        TData? data = default,
        string? message = null,
        IEnumerable<Func<ContextWrapper<HttpContext>, Task>>? formatters = null)
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

    public StandardJsonHttpResult<TData> WithData(TData? data)
    {
        Data = data;

        return this;
    }
}
