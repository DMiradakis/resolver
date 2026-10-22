using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Resolver.Config;
using Xunit;

namespace Resolver.Tests;

public class ConfigServiceTests
{
    [Fact]
    public void GetActiveProfileConfig_WhenProfileExists_ReturnsThatProfile()
    {
        // Arrange
        var expected = new ResolverConfig.ResolverProfile
        {
            ProjectRootDirectory = "projects"
        };

        var config = new ResolverConfig
        {
            ActiveProfile = "editing",
            Profiles = new() { ["editing"] = expected }
        };

        var service = new ConfigService(
            NullLogger<ConfigService>.Instance,
            Options.Create(config));

        // Act
        var actual = service.GetActiveProfileConfig();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void GetActiveProfileConfig_WhenActiveProfileDoesNotExist_Throws()
    {
        // Arrange
        var config = new ResolverConfig
        {
            ActiveProfile = "missing",
            Profiles = new() { ["editing"] = new ResolverConfig.ResolverProfile() }
        };

        var service = new ConfigService(
            NullLogger<ConfigService>.Instance,
            Options.Create(config));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.GetActiveProfileConfig());
    }
}
