// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.ModuleInitializer.AssemblyLoader;

namespace Teronis.ModuleInitializer.AssemblyLoader
{
    public static class AssemblyLoaderInjectorExtensions
    {
        public static void InjectAssemblyLoader(this AssemblyLoaderInjector injector, string injectionTargetAssemblyPath, string sourceAssemblyPath) =>
            injector.InjectAssemblyInitializer(injectionTargetAssemblyPath, sourceAssemblyPath, null);
    }
}
