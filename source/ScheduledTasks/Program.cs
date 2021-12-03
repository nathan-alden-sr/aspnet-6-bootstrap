using Company.Product.WebApi.Common;
using Company.Product.WebApi.Data;
using Company.Product.WebApi.ScheduledTasks;
using Company.Product.WebApi.ScheduledTasks.Jobs;
using Company.Product.WebApi.ScheduledTasks.Logging;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Quartz;
using Serilog;

// Roslyn incorrectly says the IDE0058 suppression is unnecessary
#pragma warning disable IDE0079
#pragma warning disable IDE0058

var host =
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(
            (context, builder) => builder
                .AddUserSecrets<Worker>()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", false, true))
        .ConfigureServices(
            (context, services) =>
            {
                /*
                 * Helper methods
                 */

                void ConfigureDbContext<T>(string connectionStringName)
                    where T : DbContext
                {
                    connectionStringName = $"{context.HostingEnvironment.EnvironmentName}-{connectionStringName}";

                    var connectionString = context.Configuration.GetConnectionString(connectionStringName);

                    if (string.IsNullOrEmpty(connectionString))
                    {
                        ThrowInvalidOperationException($"A connection string named {connectionStringName} was not found.");
                    }

                    services.AddDbContextPool<T>(
                        options => options
                            .UseNpgsql(connectionString, optionsBuilder => optionsBuilder.UseNodaTime())
                            .UseSnakeCaseNamingConvention());
                }

                /*
                 * General purpose
                 */

                services
                    .AddSingleton<IClock>(SystemClock.Instance)
                    .AddScoped<IClockSnapshot, ClockSnapshot>()
                    .AddSingleton<IGuidFactory, GuidFactory>();

                /*
                 * Entity Framework Core
                 */

                ConfigureDbContext<DatabaseContext>("company-product");

                /*
                 * Quartz.NET
                 */

                services.Configure<QuartzOptions>(
                    options =>
                    {
                        options.Scheduling.IgnoreDuplicates = true;
                        options.Scheduling.OverWriteExistingData = true;
                    });

                services.AddQuartz(
                    configurator =>
                    {
                        configurator.SchedulerName = typeof(Worker).Namespace!;

                        configurator.UseMicrosoftDependencyInjectionJobFactory();
                        configurator.UseDefaultThreadPool(Environment.ProcessorCount);

                        configurator
                            .AddJob<SampleJob>(
                                options => options
                                    .WithIdentity(nameof(SampleJob))
                                    .WithDescription("A sample job.")
                                    .StoreDurably())
                            .AddTrigger(
                                triggerConfigurator => triggerConfigurator
                                    .WithIdentity("Every 10 seconds")
                                    .ForJob(nameof(SampleJob))
                                    .StartNow()
                                    .WithSimpleSchedule(a => a.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever()));
                    });

                services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

                /*
                 * Worker
                 */

                services.AddHostedService<Worker>();
            })
        .UseSerilog(
            (context, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.With<UtcTimestampEnricher>())
        .Build();

await host.RunAsync();
