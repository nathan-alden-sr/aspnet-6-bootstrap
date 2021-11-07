using Company.Product.WebApi.Common;
using Company.Product.WebApi.Data;
using Company.Product.WebApi.ScheduledTasks;
using Company.Product.WebApi.ScheduledTasks.Jobs;
using Company.Product.WebApi.ScheduledTasks.Logging;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Quartz;
using Serilog;

IHost host =
    Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(
            builder =>
                builder
                    .AddUserSecrets<Worker>()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", false, true))
        .ConfigureServices(
            (context, services) =>
            {
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

                const string connectionStringAlias = "company_product";
                string connectionString = context.Configuration.GetConnectionString(connectionStringAlias);

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    ThrowInvalidOperationException(
                        $@"Connection string alias ""{connectionStringAlias}"" not found. Use ""dotnet user-secrets"" against this project to store the connection string securely.");
                }

                services.AddDbContextPool<DatabaseContext>(
                    options => options.UseNpgsql(connectionString, optionsBuilder => optionsBuilder.UseNodaTime()).UseSnakeCaseNamingConvention());

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
                                options =>
                                {
                                    options
                                        .WithIdentity(nameof(SampleJob))
                                        .WithDescription("A sample job.")
                                        .StoreDurably();
                                })
                            .AddTrigger(
                                triggerConfigurator =>
                                {
                                    triggerConfigurator
                                        .WithIdentity("Every 10 seconds")
                                        .ForJob(nameof(SampleJob))
                                        .StartNow()
                                        .WithSimpleSchedule(a => a.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever());
                                });
                    });

                services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

                /*
                 * Worker
                 */

                services.AddHostedService<Worker>();
            })
        .UseSerilog(
            (context, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.With<UtcTimestampEnricher>();
            })
        .Build();

await host.RunAsync();