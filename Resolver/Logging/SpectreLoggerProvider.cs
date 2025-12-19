using Microsoft.Extensions.Logging;

namespace Resolver.Logging
{
    public sealed class SpectreLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
            => new SpectreLogger(categoryName);

        public void Dispose() { }
    }
}
