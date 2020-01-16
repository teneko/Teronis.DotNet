using System;
using System.IO;

namespace Teronis.IO
{
    public static class DirectoryTools
    {
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

        public static DirectoryInfo GetDirectoryOfFileAbove(string fileNameWithExtension, string directory, bool includePassedDirectory = false)
            => GetDirectoryOfFileAbove(fileNameWithExtension, new DirectoryInfo(directory));
    }
}
