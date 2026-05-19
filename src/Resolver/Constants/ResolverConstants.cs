namespace Resolver.Constants
{
    public static class ResolverConstants
    {
        public const string AppName = "Resolver";
        public const string ConfigFileName = "resolver.config.json";
        public const string ProjectExportFolderName = "Exports";
        public static readonly string[] ScaffoldSubfolders =
        [
            "Footage",
            "Audio",
            "Exports",
            "Cache",
            "Proxies",
            "Gallery",
            "ResolveProjectFiles",
        ];

        public static class ConfigKeyMappings
        {
            public static class ProjectRootDirectoryMapping
            {
                public const string JsonKey = "projectRootDirectory";
                public const string FriendlyKey = "projectRootDirectory";
            }

            public static class ProjectArchiveRootDirectoryMapping
            {
                public const string JsonKey = "projectArchiveRootDirectory";
                public const string FriendlyKey = "projectArchiveRootDirectory";
            }

            public static class ProjectExportRootDirectoryMapping
            {
                public const string JsonKey = "projectExportRootDirectory";
                public const string FriendlyKey = "projectExportRootDirectory";
            }
        }
    }
}
