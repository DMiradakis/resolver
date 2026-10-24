namespace Resolver.Versioning
{
    public sealed class UpdateSettings
    {
        public const string SectionName = "Update";

        public string LatestReleaseApiUrl { get; set; } = string.Empty;
        public string ReleasesPageUrl { get; set; } = string.Empty;
    }
}
