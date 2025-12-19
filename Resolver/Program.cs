using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resolver.Config;
using Resolver.Constants;
using Resolver.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel.Design.Serialization;

#region Configure Spectre Console decorator.

var figletText = new FigletText(ResolverConstants.AppName).LeftJustified().Color(Color.Blue);
AnsiConsole.Write(figletText);

#endregion

var builder = Host.CreateApplicationBuilder(args);

#region Read and register resolver config file.

var resolverConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(ResolverConstants.ConfigFileName, optional: true)
    .Build();

var resolverConfigBindingInstance = new ResolverConfig();
resolverConfiguration.Bind(resolverConfigBindingInstance);
builder.Services.AddSingleton(resolverConfigBindingInstance);

#endregion

var registrar = new TypeRegistrar(builder.Services);
var app = new CommandApp(registrar);

//var app = builder.Build();
//var logger = app.Services.GetRequiredService<ILogger<Program>>();
//logger.LogInformation("Hello world!");