using Spectre.Console.Cli;

namespace Resolver.Config
{
    public class ProfileNameSettings : CommandSettings
    {
        [CommandArgument(0, "[ProfileName]")]
        public string ProfileName { get; set; } = string.Empty;
    }
}
