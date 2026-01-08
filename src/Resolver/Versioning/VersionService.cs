using System.Reflection;

namespace Resolver.Versioning
{
    public class VersionService
    {
        public static string GetVersion()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? "Unknown Version";
        }
    }
}
