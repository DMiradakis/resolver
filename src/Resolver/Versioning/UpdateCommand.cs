using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.Versioning;
using Spectre.Console.Cli;

namespace Resolver.Versioning
{
    public class UpdateCommand : AsyncCommand
    {
        private readonly ILogger<UpdateCommand> _logger;
        private readonly UpdateSettings _settings;

        public UpdateCommand(ILogger<UpdateCommand> logger, IOptions<UpdateSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Resolver");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

            try
            {
                using var response = await client.GetAsync(_settings.LatestReleaseApiUrl, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Could not check for updates. GitHub returned {statusCode}.", response.StatusCode);
                    return 1;
                }

                var release = await response.Content.ReadFromJsonAsync<GitHubRelease>(
                    cancellationToken: cancellationToken);

                if (string.IsNullOrWhiteSpace(release?.TagName))
                {
                    _logger.LogError("Could not check for updates. The GitHub release response did not contain a version tag.");
                    return 1;
                }

                var latestVersion = release.TagName;
                var currentVersion = VersionService.GetVersion();
                var parsedLatest = ParseVersion(latestVersion);
                var parsedCurrent = ParseVersion(currentVersion);
                if (parsedLatest is null || parsedCurrent is null)
                {
                    _logger.LogError(
                        "Could not compare versions. Current version: {currentVersion}; latest release: {latestVersion}.",
                        currentVersion,
                        latestVersion);
                    return 1;
                }

                if (parsedLatest.CompareTo(parsedCurrent) > 0)
                {
                    _logger.LogInformation(
                        "An update is available: {latestVersion} (current: {currentVersion}). Download it from {releaseUrl}",
                        latestVersion,
                        currentVersion,
                        _settings.ReleasesPageUrl);
                }
                else
                {
                    _logger.LogInformation("Resolver is up to date ({currentVersion}).", currentVersion);
                }

                return 0;
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Could not connect to GitHub to check for updates.");
                return 1;
            }
            catch (JsonException exception)
            {
                _logger.LogError(exception, "GitHub returned an invalid response while checking for updates.");
                return 1;
            }
        }

        private static NuGetVersion? ParseVersion(string value)
        {
            var normalizedValue = value.Trim().TrimStart('v', 'V');
            return NuGetVersion.TryParse(normalizedValue, out var version) ? version : null;
        }

        private sealed record GitHubRelease(
            [property: JsonPropertyName("tag_name")] string? TagName);
    }
}
