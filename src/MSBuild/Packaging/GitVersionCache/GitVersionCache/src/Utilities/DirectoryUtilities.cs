using System;
using System.IO;

namespace Teronis.GitVersionCache.Utilities
{
    public static class DirectoryUtilities
    {
        /// <summary>
        /// Get the absolute directory path of file above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includePassedDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfFileAbove(string fileNameWithExtension, DirectoryInfo directory, bool includePassedDirectory = false)
        {
            directory = directory ?? throw new ArgumentNullException(nameof(directory));

            if (!includePassedDirectory) {
                directory = directory.Parent;
            }

            for (; ; ) {
                if (directory == null) {
                    return null;
                }

                var rootMarkerFile = Path.Combine(directory.FullName, fileNameWithExtension);

                if (File.Exists(rootMarkerFile)) {
                    return directory;
                } else {
                    directory = directory.Parent;
                }
            }
        }

        /// <summary>
        /// Get the absolute directory path of file above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includePassedDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfFileAbove(string fileNameWithExtension, string directory, bool includePassedDirectory = false) =>
            GetDirectoryOfFileAbove(fileNameWithExtension, new DirectoryInfo(directory), includePassedDirectory: includePassedDirectory);

        /// <summary>
        /// Get the absolute directory path of file above beginning from the parent directory of the entry point, unless <paramref name="includePassedDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfFileAbove(string fileNameWithExtension, bool includePassedDirectory = false) =>
            GetDirectoryOfFileAbove(fileNameWithExtension, AppDomain.CurrentDomain.BaseDirectory, includePassedDirectory: includePassedDirectory);
    }
}
