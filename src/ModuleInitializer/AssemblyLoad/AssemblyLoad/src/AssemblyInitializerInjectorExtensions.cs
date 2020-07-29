namespace Teronis.NetCoreApp.AssemblyLoadInjection
{
    public static class AssemblyInitializerInjectorExtensions
    {
        public static void InjectAssemblyInitializer(this AssemblyInitializerInjector injector, string injectionTargetAssemblyPath, string assemblyNameToBeLoaded) =>
            injector.InjectAssemblyInitializer(injectionTargetAssemblyPath, assemblyNameToBeLoaded, null);
    }
}
