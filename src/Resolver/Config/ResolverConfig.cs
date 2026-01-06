using System.Text.Json.Serialization;

namespace Resolver.Config
{
    public class ResolverConfig
    {
        [JsonPropertyName("activeProfile")]
        public string ActiveProfile { get; set; } = string.Empty;

        [JsonPropertyName("profiles")]
        public Dictionary<string, ResolverProfile> Profiles { get; set; } = [];

        public class ResolverProfile
        {
            [JsonPropertyName("projectRootDirectory")]
            public string ProjectRootDirectory { get; set; } = string.Empty;

            [JsonPropertyName("projectArchiveRootDirectory")]
            public string ProjectArchiveRootDirectory { get; set; } = string.Empty;

            [JsonPropertyName("projectExportRootDirectory")]
            public string ProjectExportRootDirectory { get; set; } = string.Empty;
        }
    }
}
