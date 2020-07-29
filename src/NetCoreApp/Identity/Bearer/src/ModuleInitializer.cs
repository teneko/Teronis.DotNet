using Teronis.Mvc.Hosting;

namespace Teronis.Identity.Bearer
{
    public static class ModuleInitializer
    {
        public static void Initialize() =>
            HostingStartupAssemblies.InjectHostingStartup(typeof(ModuleInitializer));
    }
}
