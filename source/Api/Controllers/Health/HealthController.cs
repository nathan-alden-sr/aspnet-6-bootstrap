using Company.Product.WebApi.Api.Caching;
using Company.Product.WebApi.Api.Results;
using Company.Product.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;

namespace Company.Product.WebApi.Api.Controllers.Health;

[Route("health")]
public sealed class HealthController : ControllerBase
{
    private static readonly Guid DatabaseHealthCacheKey = Guid.NewGuid();
    private readonly DatabaseContext _databaseContext;
    private readonly IMemoryCache _memoryCache;

    public HealthController(IMemoryCache memoryCache, DatabaseContext databaseContext)
    {
        ThrowIfNull(memoryCache, nameof(memoryCache));
        ThrowIfNull(databaseContext, nameof(databaseContext));

        _memoryCache = memoryCache;
        _databaseContext = databaseContext;
    }

    [HttpGet]
    [SwaggerOperation("Reports on the health of the API.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IStandardJsonResult<GetHealthResultData>))]
    public async Task<IStandardJsonResult> GetAsync(CancellationToken cancellationToken = default)
    {
        DatabaseHealth databaseHealth =
            await _memoryCache.ThreadSafeLazyGetOrCreate(
                DatabaseHealthCacheKey,
                async entry =>
                {
                    try
                    {
                        _ = await _databaseContext.Database.ExecuteSqlRawAsync("SELECT NULL;", cancellationToken);

                        return DatabaseHealth.Healthy;
                    }
                    catch (Exception ex)
                    {
                        _ = ex;
                        return DatabaseHealth.Unhealthy;
                    }
                },
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });
        GetHealthResultData resultData = new(ApiHealth.Healthy, databaseHealth);
        bool healthy = resultData.ApiHealth == ApiHealth.Healthy && resultData.DatabaseHealth == DatabaseHealth.Healthy;

        return Result.Ok().AsStandardJson(resultData).WithMessage($"The API is {(healthy ? "" : "not ")}functioning normally.");
    }
}