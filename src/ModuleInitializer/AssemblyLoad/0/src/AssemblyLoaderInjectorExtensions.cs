namespace Teronis.ModuleInitializer.AssemblyLoad
{
    public static class AssemblyLoaderInjectorExtensions
    {
        public static void InjectAssemblyInitializer(this AssemblyLoaderInjector injector, string injectionTargetAssemblyPath, string sourceAssemblyPath) =>
            injector.InjectAssemblyInitializer(injectionTargetAssemblyPath, sourceAssemblyPath, null);
    }
}
