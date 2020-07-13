using System;
using System.Reflection;
using System.Text;
using Teronis.Text;

namespace Teronis.Mvc.Hosting
{
    public static class HostingStartupAssemblies
    {
        public const string AspNetCoreHostingStartupAssembliesEnvironmentVariableName = "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES";

        public static void InjectHostingStartup(string fullAssemblyName)
        {
            fullAssemblyName = fullAssemblyName ?? throw new ArgumentNullException(nameof(fullAssemblyName));
            var environmentVariableName = AspNetCoreHostingStartupAssembliesEnvironmentVariableName;
            var assembliesString = Environment.GetEnvironmentVariable(environmentVariableName) ?? string.Empty;
            var splitedAssemblies = assembliesString.Split(';');
            var joinedAssembliesBuilder = new StringBuilder();
            var stringSeparater = new StringSeparationHelper(";");

            void appendAssembly(string assembly)
            {
                joinedAssembliesBuilder.Append(assembly);
                stringSeparater.SetStringSeparator(joinedAssembliesBuilder);
            }

            foreach (var splittedAssembly in splitedAssemblies) {
                appendAssembly(splittedAssembly);
            }

            appendAssembly(fullAssemblyName);
            var newAssembliesString = joinedAssembliesBuilder.ToString();
            Environment.SetEnvironmentVariable(environmentVariableName, newAssembliesString);
        }

        public static void InjectHostingStartup(Assembly assembly)
        {
            assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            InjectHostingStartup(assembly.FullName!);
        }

        public static void InjectHostingStartup(Type typeInsideAssembly)
        {
            typeInsideAssembly = typeInsideAssembly ?? throw new ArgumentNullException(nameof(typeInsideAssembly));
            InjectHostingStartup(typeInsideAssembly.Assembly);
        }
    }
}
