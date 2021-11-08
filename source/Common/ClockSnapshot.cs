using System.Diagnostics;
using NodaTime;

namespace Company.Product.WebApi.Common;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[DebuggerTypeProxy(typeof(DebugView))]
public sealed partial class ClockSnapshot : IClockSnapshot
{
    private readonly Instant _instant;

    public ClockSnapshot(IClock clock)
    {
        ThrowIfNull(clock, nameof(clock));

        _instant = clock.GetCurrentInstant();
    }

    private string DebuggerDisplay => _instant.ToString();

    public Instant GetCurrentInstant() =>
        _instant;
}