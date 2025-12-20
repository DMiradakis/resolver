using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resolver.Constants;
using System.Text.Json;

namespace Resolver.Config
{
    public class ConfigService
    {
        private readonly ILogger<ConfigService> _logger;
        private readonly IOptions<ResolverConfig> _config;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ConfigService(ILogger<ConfigService> logger, IOptions<ResolverConfig> config)
        {
            _logger = logger;
            _config = config;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public static string? GetAppDirectory()
        {
            var exePath = Environment.ProcessPath;
            return Path.GetDirectoryName(exePath);
        }

        public static string? GetConfigPath()
        {
            var appDirectory = GetAppDirectory();
            var localPath = Path.Combine(
                appDirectory,
                ResolverConstants.ConfigFileName);

            if (File.Exists(localPath))
                return localPath;

            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ResolverConstants.AppName,
                ResolverConstants.ConfigFileName);

            if (File.Exists(appDataPath))
                return appDataPath;

            return null;
        }

        public ResolverConfig.ResolverProfile GetActiveProfileConfig()
        {
            var configExists = _config.Value.Profiles.TryGetValue(_config.Value.ActiveProfile, out var profileConfig);
            if (!configExists)
            {
                throw new Exception("An active config profile has not been selected. Please set an active config profile.");
            }
            if (profileConfig is null)
            {
                throw new Exception("The selected config profile is NULL. Please set the profile's config values.");
            }

            return profileConfig;
        }

        public void SetActiveProfile(string profileName)
        {
            var configPath = GetConfigPath();
            var configJson = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<ResolverConfig>(configJson);
            _logger.LogInformation("The current json is: {json}", configJson);
            config.ActiveProfile = profileName;
            var updatedConfigJson = JsonSerializer.Serialize(config, _jsonSerializerOptions);
            _logger.LogInformation("The updated json is: {json}", updatedConfigJson);
            File.WriteAllText(configPath, updatedConfigJson);
        }
    }
}
