namespace Company.Product.WebApi.Api.Results;

public interface IStandardJsonResult<TContext, out TData> : IStandardJsonResult<TContext>
    where TContext : class
{
    TData? Data { get; }
}
