using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace Resolver.Versioning
{
    public class DisplayVersionCommand : Command
    {
        private readonly ILogger<DisplayVersionCommand> _logger;

        public DisplayVersionCommand(ILogger<DisplayVersionCommand> logger)
        {
            _logger = logger;
        }

        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            var version = VersionService.GetVersion();
            _logger.LogInformation("{v}", version);
            return 0;
        }
    }
}
