using Company.Product.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Company.Product.WebApi.ScheduledTasks.Jobs;

public sealed class SampleJob : IJob
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger<SampleJob> _logger;

    public SampleJob(ILogger<SampleJob> logger, DatabaseContext databaseContext)
    {
        ThrowIfNull(logger, nameof(logger));
        ThrowIfNull(databaseContext, nameof(databaseContext));

        _logger = logger;
        _databaseContext = databaseContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogDebug("Starting sample job");

        await _databaseContext.Database.ExecuteSqlRawAsync("SELECT NULL;");

        _logger.LogInformation("Completed sample job");
    }
}