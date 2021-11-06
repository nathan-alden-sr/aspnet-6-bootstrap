using Serilog.Core;
using Serilog.Events;

namespace Company.Product.WebApi.Api.Logging;

public sealed class UtcTimestampEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) =>
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UtcTimestamp", logEvent.Timestamp.ToUniversalTime().ToString("O")));
}