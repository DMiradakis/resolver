using Spectre.Console.Cli;

namespace Resolver.Projects
{
    public class ArchiveProjectCommand : Command<ArchiveProjectCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[ProjectName]")]
            public string ProjectName { get; set; } = string.Empty;
        }

        private readonly ProjectsService _projectsService;

        public ArchiveProjectCommand(ProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            _projectsService.ArchiveProject(settings.ProjectName);
            return 0;
        }
    }
}
