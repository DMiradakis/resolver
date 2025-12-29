using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ConfigInitCommand : Command
    {
        private readonly ConfigService _configService;
        public ConfigInitCommand(ConfigService configService)
        {
            _configService = configService;
        }
        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            _configService.InitConfig();
            return 0;
        }
    }
}
