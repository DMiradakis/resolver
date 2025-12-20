using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ConfigInspectCommand : Command
    {
        private readonly ConfigService _configService;

        public ConfigInspectCommand(ConfigService configService)
        {
            _configService = configService;
        }

        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            _configService.InspectConfig();
            return 0;
        }
    }
}
