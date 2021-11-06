namespace Company.Product.WebApi.Api.Controllers.Health;

public sealed record GetHealthResultData(ApiHealth ApiHealth, DatabaseHealth DatabaseHealth);