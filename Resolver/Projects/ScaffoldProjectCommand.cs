using Spectre.Console.Cli;

namespace Resolver.Projects
{
    public class ScaffoldProjectCommand : Command<ProjectSettings>
    {
        private readonly ProjectsService _projectsService;

        public ScaffoldProjectCommand(ProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        public override int Execute(CommandContext context, ProjectSettings settings, CancellationToken cancellationToken)
        {
            _projectsService.ScaffoldProject(settings.ProjectName);
            return 0;
        }
    }
}
