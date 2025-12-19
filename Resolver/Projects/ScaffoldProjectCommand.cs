using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace Resolver.Projects
{
    public class ScaffoldProjectCommand : Command<ScaffoldProjectCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[ProjectName]")]
            public string ProjectName { get; set; } = string.Empty;
        }

        private readonly ILogger<ScaffoldProjectCommand> _logger;
        private readonly ProjectsService _projectsService;

        public ScaffoldProjectCommand(ILogger<ScaffoldProjectCommand> logger, ProjectsService projectsService)
        {
            _logger = logger;
            _projectsService = projectsService;
        }

        public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            _projectsService.ScaffoldProject(settings.ProjectName);
            return 0;
        }
    }
}
