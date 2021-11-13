using System.Text.Json.Serialization;

namespace Company.Product.WebApi.Api.Results;

public interface IResult<TContext>
    where TContext : class
{
    [JsonIgnore]
    int? StatusCode { get; }

    Task ExecuteResultAsync(ContextWrapper<TContext> context);
}
