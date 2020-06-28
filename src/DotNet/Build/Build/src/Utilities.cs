using System;
using System.IO;
using Teronis.IO;

namespace Teronis.DotNet.Build
{
    public static class Utilities
    {
        public static DirectoryInfo? GetRootDirectory() {
            var directory = AppDomain.CurrentDomain.BaseDirectory ??
                throw new ArgumentNullException("Current app domain directory is null.");

           return DirectoryTools.GetDirectoryOfFileAbove(".msbuild", directory);
        }
    }
}
