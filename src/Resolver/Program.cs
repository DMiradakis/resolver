using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resolver.Config;
using Resolver.Constants;
using Resolver.DependencyInjection;
using Resolver.Projects;
using Resolver.Versioning;
using Spectre.Console.Cli;

var builder = Host.CreateApplicationBuilder(args);

#region Read and register resolver config file.

var appDirectory = ConfigService.GetAppDirectory();
var resolverConfiguration = new ConfigurationBuilder()
    .SetBasePath(appDirectory)
    .AddJsonFile(ResolverConstants.ConfigFileName, optional: true)
    .Build();

builder.Services.Configure<ResolverConfig>(resolverConfiguration);

#endregion

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new Resolver.Logging.SpectreLoggerProvider());
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<ProjectsService>();
builder.Services.AddSingleton<VersionService>();

var registrar = new TypeRegistrar(builder.Services);
var app = new CommandApp(registrar);
app.Configure(config =>
{
    config.SetApplicationName(ResolverConstants.AppName.ToLower());
    config.AddCommand<DisplayVersionCommand>("version")
        .WithDescription("Displays the current version of the app.")
        .WithExample("version");
    config.AddBranch("project", projectConfig =>
    {
        projectConfig.AddCommand<ScaffoldProjectCommand>("scaffold")
            .WithDescription("Scaffolds a new Da Vinci Resolve project directory with subfolders.")
            .WithExample("project scaffold my-project");
        projectConfig.AddCommand<ArchiveProjectCommand>("archive")
            .WithDescription("Archives an existing Da Vinci Resolve project directory.")
            .WithExample("project archive my-project");
        projectConfig.AddCommand<ExportProjectCommand>("export")
            .WithDescription("Copies the project export files to the default export directory.")
            .WithExample("project export my-project");
    });
    config.AddBranch("config", resolverConfig =>
    {
        resolverConfig.AddBranch("profile", profileConfig =>
        {
            profileConfig.AddCommand<ConfigProfileInitCommand>("init")
                .WithDescription("Initializes a new config profile with null values.")
                .WithExample("config profile init my-profile");
            profileConfig.AddCommand<ConfigProfileSetCommand>("set")
                .WithDescription("Sets the active config profile.")
                .WithExample("config profile set my-profile");
        });
        resolverConfig.AddCommand<ConfigInitCommand>("init")
            .WithDescription("Initializes a new config file with default values.")
            .WithExample("config init");
        resolverConfig.AddCommand<ConfigInspectCommand>("inspect")
            .WithDescription("Inspects all config values.")
            .WithExample("config inspect");
        resolverConfig.AddCommand<ConfigSetCommand>("set")
            .WithDescription("Sets a config value in the active profile.")
            .WithExample("config set ProjectRootDirectory /path/to/projects");
    });
});

return app.Run(args);