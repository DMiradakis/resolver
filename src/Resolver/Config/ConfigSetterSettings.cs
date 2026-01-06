using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ConfigSetterSettings : CommandSettings
    {
        [CommandArgument(0, "[SettingsKey]")]
        public string Key { get; set; } = string.Empty;

        [CommandArgument(1, "[SettingsValue]")]
        public string Value {  get; set; } = string.Empty;
    }
}
