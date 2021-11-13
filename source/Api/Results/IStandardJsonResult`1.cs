namespace Company.Product.WebApi.Api.Results;

public interface IStandardJsonResult<TContext> : IResult<TContext>
    where TContext : class
{
    string? Message { get; }
}
