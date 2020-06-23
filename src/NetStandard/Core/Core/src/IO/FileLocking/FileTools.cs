using System;
using System.IO;

namespace Teronis.IO
{
    public static class FileTools
    {
        /// <summary>
        /// Get the absolute file path of file above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includePassedDirectory"/> is true.
        /// </summary>
        public static FileInfo GetPathOfFileAbove(string fileNameWithExtension, DirectoryInfo directory, bool includePassedDirectory = false)
        {
            var foundDirectory = DirectoryTools.GetDirectoryOfFileAbove(fileNameWithExtension, directory, includePassedDirectory);

            if (foundDirectory != null) {
                return new FileInfo(Path.Combine(foundDirectory.FullName, fileNameWithExtension));
            } else {
                return null;
            }
        }

        /// <summary>
        /// Get the absolute file path of file above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includePassedDirectory"/> is true.
        /// </summary>
        public static FileInfo GetPathOfFileAbove(string fileNameWithExtension, string directory, bool includePassedDirectory = false) => 
            GetPathOfFileAbove(fileNameWithExtension, new DirectoryInfo(directory), includePassedDirectory);

        /// <summary>
        /// Get the absolute file path of file above beginning from the parent directory of the entry point, unless <paramref name="includePassedDirectory"/> is true.
        /// </summary>
        public static FileInfo GetPathOfFileAbove(string fileNameWithExtension, bool includePassedDirectory = false) => 
            GetPathOfFileAbove(fileNameWithExtension, AppDomain.CurrentDomain.BaseDirectory, includePassedDirectory);
    }
}
