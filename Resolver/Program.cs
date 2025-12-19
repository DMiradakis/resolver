using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resolver.Config;
using Resolver.Constants;
using Resolver.DependencyInjection;
using Resolver.Projects;
using Spectre.Console.Cli;

var builder = Host.CreateApplicationBuilder(args);

#region Read and register resolver config file.

var appDirectory = ConfigService.GetAppDirectory()!;
var resolverConfiguration = new ConfigurationBuilder()
    .SetBasePath(appDirectory)
    .AddJsonFile(ResolverConstants.ConfigFileName, optional: true)
    .Build();

builder.Services.Configure<ResolverConfig>(resolverConfiguration);

#endregion

builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<ProjectsService>();

var registrar = new TypeRegistrar(builder.Services);
var app = new CommandApp(registrar);
app.Configure(config =>
{
    config.SetApplicationName(ResolverConstants.AppName.ToLower());
    config.AddCommand<ScaffoldProjectCommand>("scaffold")
        .WithDescription("Scaffolds a new Da Vinci Resolve project directory with subfolders.")
        .WithExample("scaffold my-project");
    config.AddCommand<ArchiveProjectCommand>("archive")
        .WithDescription("Archives an existing Da Vinci Resolve project directory.")
        .WithExample("archive my-project");
});

return app.Run(args);