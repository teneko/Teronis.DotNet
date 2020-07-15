using Teronis.Mvc.Hosting;

namespace Teronis.Identity
{
    public static class ModuleInitializer
    {
        public static void Initialize() =>
            HostingStartupAssemblies.InjectHostingStartup(typeof(ModuleInitializer));
    }
}
