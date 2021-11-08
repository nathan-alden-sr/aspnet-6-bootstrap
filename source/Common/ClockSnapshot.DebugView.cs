namespace Company.Product.WebApi.Common;

public sealed partial class ClockSnapshot
{
    internal sealed class DebugView
    {
        private readonly ClockSnapshot _clockSnapshot;

        public DebugView(ClockSnapshot clockSnapshot)
        {
            _clockSnapshot = clockSnapshot;
        }

        public string Instant => _clockSnapshot._instant.ToString();
    }
}