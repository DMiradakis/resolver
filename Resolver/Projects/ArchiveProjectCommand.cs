using Spectre.Console.Cli;

namespace Resolver.Projects
{
    public class ArchiveProjectCommand : Command<ProjectSettings>
    {
        private readonly ProjectsService _projectsService;

        public ArchiveProjectCommand(ProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        public override int Execute(CommandContext context, ProjectSettings settings, CancellationToken cancellationToken)
        {
            _projectsService.ArchiveProject(settings.ProjectName);
            return 0;
        }
    }
}
