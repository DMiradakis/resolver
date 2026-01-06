using Spectre.Console.Cli;

namespace Resolver.Projects
{
    public class ProjectSettings : CommandSettings
    {
        [CommandArgument(0, "[ProjectName]")]
        public string ProjectName { get; set; } = string.Empty;
    }
}
