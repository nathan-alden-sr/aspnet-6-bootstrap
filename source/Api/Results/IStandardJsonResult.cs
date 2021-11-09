namespace Company.Product.WebApi.Api.Results;

public interface IStandardJsonResult : IResult
{
    string? Message { get; }
}
