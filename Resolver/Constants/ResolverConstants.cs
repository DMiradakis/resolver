namespace Resolver.Constants
{
    public static class ResolverConstants
    {
        public const string AppName = "Resolver";
        public const string ConfigFileName = "resolver.config.json";
        public static readonly string[] ScaffoldSubfolders =
        [
            "Footage/ScreenRecordings",
            "Footage/Camera",
            "Footage/Stock",
            "Audio/Voiceover",
            "Audio/Music",
            "Audio/SFX",
            "Graphics",
            "Exports",
            "Cache",
            "Proxies",
            "Gallery",
            "ResolveProjectFiles",
            "ProjectNotes"
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
        }
    }
}
