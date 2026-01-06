using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ConfigSetCommand : Command<ConfigSetterSettings>
    {
        private ConfigService _configService;

        public ConfigSetCommand(ConfigService configService)
        {
            _configService = configService;
        }

        public override int Execute(CommandContext context, ConfigSetterSettings settings, CancellationToken cancellationToken)
        {
            var settingKey = settings.Key;
            var settingValue = settings.Value;
            _configService.SaveConfigSetting(settingKey, settingValue);
            return 0;
        }
    }
}
