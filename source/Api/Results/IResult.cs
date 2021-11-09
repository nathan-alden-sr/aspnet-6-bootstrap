using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Results;

public interface IResult : IActionResult
{
    [JsonIgnore]
    int? StatusCode { get; }

    [JsonIgnore]
    IReadOnlyCollection<Action<ActionContext>> Formatters { get; }
}
