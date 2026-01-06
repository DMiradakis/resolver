using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ConfigProfileSetCommand : Command<ProfileNameSettings>
    {
        private readonly ConfigService _configService;

        public ConfigProfileSetCommand(ConfigService configService)
        {
            _configService = configService;
        }

        public override int Execute(CommandContext context, ProfileNameSettings settings, CancellationToken cancellationToken)
        {
            _configService.SetActiveProfile(settings.ProfileName);
            return 0;
        }
    }
}
