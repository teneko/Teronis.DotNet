using System;
using System.IO;

namespace Teronis.IO
{
    public static class FileTools
    {
        public static FileInfo GetPathOfFileAbove(string fileNameWithExtension, DirectoryInfo directory, bool includePassedDirectory = false)
        {
            var foundDirectory = DirectoryTools.GetDirectoryOfFileAbove(fileNameWithExtension, directory);

            if (foundDirectory != null) {
                return new FileInfo(Path.Combine(foundDirectory.FullName, fileNameWithExtension));
            } else {
                return null;
            }
        }

        public static FileInfo GetPathOfFileAbove(string fileNameWithExtension, string directory, bool includePassedDirectory = false)
            => GetPathOfFileAbove(fileNameWithExtension, new DirectoryInfo(directory));
    }
}
