using System;
using System.IO;
using Teronis.IO;

namespace Teronis.Build
{
    public static class TeronisBuildUtils
    {
        public static DirectoryInfo? GetRootDirectory()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory ??
                throw new ArgumentNullException("Current app domain directory is null.");

            return DirectoryUtils.GetDirectoryOfFileAbove(".msbuild", directory);
        }
    }
}
