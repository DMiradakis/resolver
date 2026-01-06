using Microsoft.Extensions.Logging;
using Resolver.Config;
using Resolver.Constants;
using Spectre.Console;
using System.IO.Compression;

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

        public void ScaffoldProject(string newProjectFolderName)
        {
            var profileConfig = _configService.GetActiveProfileConfig();

            // Create project base directory.
            var projectBaseDirectory = Path.Combine(profileConfig.ProjectRootDirectory, newProjectFolderName);
            if (Directory.Exists(projectBaseDirectory))
            {
                _logger.LogError("Project directory already exists. Aborting scaffold operation.");
                return;
            }
            _ = Directory.CreateDirectory(projectBaseDirectory);

            _logger.LogInformation("Creating project subfolders...");

            // Create scaffold subdirectories.
            foreach (var folder in ResolverConstants.ScaffoldSubfolders)
            {
                Directory.CreateDirectory(Path.Combine(projectBaseDirectory, folder));
            };

            _logger.LogInformation("Project subfolders created successfully.");
        }

        public void ArchiveProject(string projectFolderName)
        {
            var profileConfig = _configService.GetActiveProfileConfig();
            var projectPath = Path.Combine(profileConfig.ProjectRootDirectory, projectFolderName);

            var zipFileName = $"{projectFolderName}.zip";
            var tempZipPath = Path.Combine(Path.GetTempPath(), zipFileName);
            var finalZipPath = Path.Combine(profileConfig.ProjectArchiveRootDirectory, zipFileName);

            _logger.LogInformation("Checking and recycling temp cache for project...");

            // Clear project cache from temp folder if exists.
            if (File.Exists(tempZipPath))
                File.Delete(tempZipPath);

            _logger.LogInformation("Creating project zip file...");

            // Create zip file in temp folder
            ZipFile.CreateFromDirectory(
                projectPath,
                tempZipPath,
                CompressionLevel.Optimal,
                includeBaseDirectory: true);

            // Delete if exists in archive
            if (File.Exists(finalZipPath))
            {
                _logger.LogWarning("Project preexists in archive. Overwriting...");
                AnsiConsole.MarkupLine(
                    $"[yellow]Archive already exists, overwriting:[/] {finalZipPath}");
                File.Delete(finalZipPath);
            }

            // Move from temp to archive
            File.Move(tempZipPath, finalZipPath);

            _logger.LogInformation("Project archived successfully.");
        }

        public void ExportProject(string projectFolderName)
        {
            var profileConfig = _configService.GetActiveProfileConfig();
            var projectPath = Path.Combine(profileConfig.ProjectRootDirectory, projectFolderName);
            var projectExportPath = Path.Combine(projectPath, ResolverConstants.ProjectExportFolderName);
            var exportPath = Path.GetFullPath(profileConfig.ProjectExportRootDirectory);

            if (!Directory.Exists(projectExportPath))
                throw new Exception("No export folder found in the project directory.");

            if (!Directory.Exists(exportPath))
                throw new Exception("The configured export path does not exist.");

            var exportFiles = Directory.GetFiles(projectExportPath);

            if (exportFiles.Length == 0)
                throw new Exception("No export files found in the project export folder.");

            foreach (var filePath in exportFiles)
            {
                var fileName = Path.GetFileName(filePath);
                var destinationPath = Path.Combine(exportPath, fileName);
                File.Copy(filePath, destinationPath, overwrite: true);
            }

            _logger.LogInformation("Project export files copied successfully to {exportPath}.", exportPath);
        }
    }
}
