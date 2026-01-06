using Spectre.Console.Cli;

namespace Resolver.Projects
{
    public class ExportProjectCommand : Command<ProjectSettings>
    {
        private readonly ProjectsService _projectsService;

        public ExportProjectCommand(ProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        public override int Execute(CommandContext context, ProjectSettings settings, CancellationToken cancellationToken)
        {
            _projectsService.ExportProject(settings.ProjectName);
            return 0;
        }
    }
}
