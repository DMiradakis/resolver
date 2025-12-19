namespace Resolver.Config
{
    public class ResolverConfig
    {
        public string ActiveProfile { get; set; } = string.Empty;

        public Dictionary<string, ResolverProfile> Profiles { get; set; } = [];

        public class ResolverProfile
        {
            public string ProjectRootDirectory { get; set; } = string.Empty;
            public string ProjectArchiveRootDirectory { get; set; } = string.Empty;
        }
    }
}
