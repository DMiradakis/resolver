using Microsoft.Extensions.Logging;
using Resolver.Config;

namespace Resolver.Projects
{
    public class ProjectsService
    {
        private readonly ILogger<ProjectsService> _logger;
        private readonly ConfigService _configService;

        public ProjectsService(ILogger<ProjectsService> logger, ConfigService configService)
        {
            _logger = logger;
            _configService = configService;
        }

        public void ScaffoldProject()
        {
            var profileConfig = _configService.GetActiveProfileConfig();

            
        }

        public void ArchiveProject()
        {
            var profileConfig = _configService.GetActiveProfileConfig();
        }
    }
}
