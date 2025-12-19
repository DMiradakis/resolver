using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resolver.Config;
using Resolver.Constants;
using Resolver.DependencyInjection;
using Resolver.Projects;
using Spectre.Console.Cli;

var builder = Host.CreateApplicationBuilder(args);

#region Read and register resolver config file.

var appDirectory = ConfigService.GetAppDirectory();
var resolverConfiguration = new ConfigurationBuilder()
    .SetBasePath(appDirectory)
    .AddJsonFile(ResolverConstants.ConfigFileName, optional: true)
    .Build();

var resolverConfigBindingInstance = new ResolverConfig();
resolverConfiguration.Bind(resolverConfigBindingInstance);
builder.Services.AddSingleton(resolverConfigBindingInstance);
builder.Services.AddSingleton<ConfigService>();

#endregion

builder.Services.AddSingleton<ProjectsService>();

var registrar = new TypeRegistrar(builder.Services);
var app = new CommandApp(registrar);

var hostApp = builder.Build();
var logger = hostApp.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("The CWD is: {CWD}", Directory.GetCurrentDirectory());
logger.LogInformation("Application directory: {appDirectory}", appDirectory);
Console.ReadLine();