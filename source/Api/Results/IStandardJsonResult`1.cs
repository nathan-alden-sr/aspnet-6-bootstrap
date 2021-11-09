namespace Company.Product.WebApi.Api.Results;

public interface IStandardJsonResult<out TData> : IStandardJsonResult
{
    TData? Data { get; }
}
