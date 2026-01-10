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

        public static string GetAppDirectory()
        {
            var exePath = Environment.ProcessPath
                ?? throw new Exception("Could not determine application directory.");
            return Path.GetDirectoryName(exePath)
                ?? throw new Exception("Could not determine application directory.");
        }

        public static string GetAppDataDirectory()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ResolverConstants.AppName);
        }

        public static string GetConfigPath()
        {
            var appDirectory = GetAppDirectory();
            var localPath = Path.Combine(
                appDirectory,
                ResolverConstants.ConfigFileName);

            if (File.Exists(localPath))
                return localPath;

            var appDataDirectory = GetAppDataDirectory();
            var appDataPath = Path.Combine(appDataDirectory,ResolverConstants.ConfigFileName);

            if (File.Exists(appDataPath))
                return appDataPath;

            throw new Exception(
                "Could not find a config file in either the application directory or the AppData directory. Please initialize a config file."
                );
        }

        public ResolverConfig.ResolverProfile GetActiveProfileConfig()
        {
            var configExists = _config.Value.Profiles.TryGetValue(_config.Value.ActiveProfile, out var profileConfig);
            if (!configExists)
                throw new Exception("An active config profile has not been selected. Please set an active config profile.");

            if (profileConfig is null)
                throw new Exception("The selected config profile is NULL. Please set the profile's config values.");

            return profileConfig;
        }

        public void SetActiveProfile(string profileName)
        {
            var configPath = GetConfigPath();
            var configJson = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<ResolverConfig>(configJson)
                ?? throw new Exception("Could not read config file. Aborting operation.");

            var profileExists = config.Profiles.TryGetValue(profileName, out _);
            if (!profileExists)
                throw new Exception($"The profile name {profileName} does not exist. Aborting operation.");

            config.ActiveProfile = profileName;
            var updatedConfigJson = JsonSerializer.Serialize(config, _jsonSerializerOptions);
            File.WriteAllText(configPath, updatedConfigJson);
            _logger.LogInformation("Active profile set to {pn} successfully.", profileName);
        }

        public void InitProfile(string profileName)
        {
            var configPath = GetConfigPath();
            var configJson = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<ResolverConfig>(configJson)
                ?? throw new Exception("Could not read config file. Aborting operation.");

            // Check if profile already exists.
            var profileExists = config.Profiles.TryGetValue(profileName, out _);
            if (profileExists)
                throw new Exception($"The profile name {profileName} already exists. Aborting operation.");

            config.Profiles.TryAdd(profileName, new ResolverConfig.ResolverProfile());
            var updatedJsonConfig = JsonSerializer.Serialize(config, _jsonSerializerOptions);
            File.WriteAllText(configPath, updatedJsonConfig);
            _logger.LogInformation("Profile initialized successfully.");
        }

        public void InspectConfig()
        {
            var configPath = GetConfigPath();
            var configJson = File.ReadAllText(configPath);
            _logger.LogInformation("Config file values:");
            _logger.LogInformation("{json}", configJson);
            _logger.LogInformation("Config file location: {path}", configPath);
        }

        public void InitConfig()
        {
            var appDirectoryPath = GetAppDirectory();
            var appDirectoryConfigPath = Path.Combine(appDirectoryPath, ResolverConstants.ConfigFileName);
            if (File.Exists(appDirectoryConfigPath))
                throw new Exception($"A config file already exists at {appDirectoryConfigPath}.");

            var appDataDirectoryPath = GetAppDataDirectory();
            var appDataDirectoryConfigPath = Path.Combine(appDataDirectoryPath, ResolverConstants.ConfigFileName);
            if (File.Exists(appDataDirectoryConfigPath))
                throw new Exception($"A config file already exists at {appDataDirectoryConfigPath}.");

            var resolverConfig = new ResolverConfig();
            var initialJsonConfig = JsonSerializer.Serialize(resolverConfig, _jsonSerializerOptions);
            File.WriteAllText(appDirectoryConfigPath, initialJsonConfig);
            _logger.LogInformation("Config file initialized successfully at {path}.", appDirectoryConfigPath);
        }

        private void WriteInMemoryConfigBackToFile()
        {            
            var updatedConfig = JsonSerializer.Serialize(_config.Value, _jsonSerializerOptions);
            var configPath = GetConfigPath();
            File.WriteAllText(configPath, updatedConfig);
        }

        public void SaveConfigSetting(string configSetterKey, string configSetterValue)
        {
            var currentConfig = GetActiveProfileConfig();

            if (configSetterKey == ResolverConstants.ConfigKeyMappings.ProjectRootDirectoryMapping.JsonKey)
            {
                var path = Path.GetFullPath(configSetterValue);
                currentConfig.ProjectRootDirectory = path;
                WriteInMemoryConfigBackToFile();
                _logger.LogInformation("Config key {key} updated successfully.", configSetterKey);
                return;
            }
            else if (configSetterKey == ResolverConstants.ConfigKeyMappings.ProjectArchiveRootDirectoryMapping.JsonKey)
            {
                var path = Path.GetFullPath(configSetterValue);
                currentConfig.ProjectArchiveRootDirectory = path;
                WriteInMemoryConfigBackToFile();
                _logger.LogInformation("Config key {key} updated successfully.", configSetterKey);
                return;
            }
            else if (configSetterKey == ResolverConstants.ConfigKeyMappings.ProjectExportRootDirectoryMapping.JsonKey)
            {
                var path = Path.GetFullPath(configSetterValue);
                currentConfig.ProjectExportRootDirectory = path;
                WriteInMemoryConfigBackToFile();
                _logger.LogInformation("Config key {key} updated successfully.", configSetterKey);
                return;
            }
            else
            {
                string[] configKeys = [ ResolverConstants.ConfigKeyMappings.ProjectRootDirectoryMapping.JsonKey,
                                 ResolverConstants.ConfigKeyMappings.ProjectArchiveRootDirectoryMapping.JsonKey ];
                throw new Exception($"Invalid config key: {configSetterKey}. Valid keys are: {string.Join(", ", configKeys)}");
            }
        }

        #region Deprecated

        private (string, string) SplitConfigSetterIntoKeyAndValue(string configSetterString)
        {
            // Validate input.
            if (string.IsNullOrEmpty(configSetterString))
                throw new Exception("No config key-value pair provided.");

            // Validate split.
            var parts = configSetterString.Split('=', 2);
            if (parts.Length != 2)
                throw new Exception("Expected format: key=value");

            var configSetterKey = parts[0];
            var configSetterValue = parts[1];

            // Validate key.
            if (string.IsNullOrEmpty(configSetterKey))
                throw new Exception("Config key cannot be empty.");

            // Validate value.
            if (string.IsNullOrEmpty(configSetterValue))
                throw new Exception("Config value cannot be empty.");

            return (configSetterKey, configSetterValue);
        }

        #endregion
    }
}