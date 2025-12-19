using Microsoft.Extensions.Options;
using Resolver.Constants;

namespace Resolver.Config
{
    public class ConfigService
    {
        private readonly IOptions<ResolverConfig> _config;

        public ConfigService(IOptions<ResolverConfig> config)
        {
            _config = config;
        }

        public static string? GetConfigPath()
        {
            var localPath = Path.Combine(
                Directory.GetCurrentDirectory(),
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
    }
}
