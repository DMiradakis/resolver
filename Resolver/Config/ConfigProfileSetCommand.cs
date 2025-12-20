using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ConfigProfileSetCommand : Command<ConfigProfileSetCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[ProfileName]")]
            public string ProfileName { get; set; } = string.Empty;
        }

        private readonly ConfigService _configService;

        public ConfigProfileSetCommand(ConfigService configService)
        {
            _configService = configService;
        }

        public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            _configService.SetActiveProfile(settings.ProfileName);
            return 0;
        }
    }
}
