using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Teronis.ModuleInitializer.AssemblyLoader.Utils
{
    public static class ModuleDefinitionUtils
    {
        public static IEnumerable<MethodDefinition> FindModuleInitializerMethods(ModuleDefinition module)
        {
            var moduleInitializerClasses = module
                .GetTypes()
                .Where(x => x.Name == "ModuleInitializer").ToList();

            if (moduleInitializerClasses.Count == 0) {
                throw new InjectionException(@"Could not find any type with the name 'ModuleInitializer' and content
public static class ModuleInitializer
{
    public static void Initialize()
    {
        //code goes here
    }
}");
            }

            foreach (var moduleInitializer in moduleInitializerClasses) {
                var initializeMethod = moduleInitializer.Methods.FirstOrDefault(x => x.Name == "Initialize");

                if (initializeMethod == null) {
                    throw new InjectionException($"Could not find the method 'Initialize' in type '{moduleInitializer.FullName}'.");
                }

                if (!initializeMethod.IsPublic) {
                    throw new InjectionException($"The method '{initializeMethod.FullName}' is not public.");
                }

                if (!initializeMethod.IsStatic) {
                    throw new InjectionException($"The method '{initializeMethod.FullName}' is not static.");
                }

                if (initializeMethod.Parameters.Count > 0) {
                    throw new InjectionException($"The method '{initializeMethod.FullName}' cannot have parameters.");
                }

                yield return initializeMethod;
            }
        }
    }
}
