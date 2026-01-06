using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ConfigProfileInitCommand : Command<ProfileNameSettings>
    {
        private readonly ConfigService _configService;

        public ConfigProfileInitCommand(ConfigService configService)
        {
            _configService = configService;
        }

        public override int Execute(CommandContext context, ProfileNameSettings settings, CancellationToken cancellationToken)
        {
            _configService.InitProfile(settings.ProfileName);
            return 0;
        }
    }
}