using System;
using System.IO;

namespace Teronis.IO
{
    public static class DirectoryTools
    {
        /// <summary>
        /// Get the absolute directory path of file or directory above beginning from the parent directory of <paramref name="beginningDirectory"/>, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfPathAbove(DirectoryOfPathIsAboveDelegate isDirectoryOfPathAbove, DirectoryInfo beginningDirectory, bool includeBeginningDirectory = false)
        {
            beginningDirectory = beginningDirectory ?? throw new ArgumentNullException(nameof(beginningDirectory));

            if (!includeBeginningDirectory) {
                beginningDirectory = beginningDirectory.Parent;
            }

            for (; ; ) {
                if (beginningDirectory == null) {
                    return null;
                }

                if (isDirectoryOfPathAbove(beginningDirectory)) {
                    return beginningDirectory;
                } else {
                    beginningDirectory = beginningDirectory.Parent;
                }
            }
        }

        private static bool isDirectoryOfFileAbove(DirectoryInfo directoryInfo, string fileNameWithExtension)
        {
            var directoryWithFileName = Path.Combine(directoryInfo.FullName, fileNameWithExtension);
            return File.Exists(directoryWithFileName);
        }

        /// <summary>
        /// Get the absolute directory path of file above beginning from the parent directory of <paramref name="beginningDirectory"/>, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfFileAbove(string fileNameWithExtension, DirectoryInfo beginningDirectory, bool includeBeginningDirectory = false) =>
            GetDirectoryOfPathAbove((directoryInfo) => isDirectoryOfFileAbove(directoryInfo, fileNameWithExtension), beginningDirectory, includeBeginningDirectory);

        /// <summary>
        /// Get the absolute directory path of file above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfFileAbove(string fileNameWithExtension, string directory, bool includeBeginningDirectory = false) =>
            GetDirectoryOfFileAbove(fileNameWithExtension, new DirectoryInfo(directory), includeBeginningDirectory: includeBeginningDirectory);

        /// <summary>
        /// Get the absolute directory path of file above beginning from the parent directory of the entry point, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfFileAbove(string fileNameWithExtension, bool includeBeginningDirectory = false) =>
            GetDirectoryOfFileAbove(fileNameWithExtension, AppDomain.CurrentDomain.BaseDirectory, includeBeginningDirectory);

        private static bool isDirectoryOfDirectoryAbove(DirectoryInfo directoryInfo, string directoryName)
        {
            var directoryWithFileName = Path.Combine(directoryInfo.FullName, directoryName);
            return Directory.Exists(directoryWithFileName);
        }

        /// <summary>
        /// Get the absolute directory path of directory above beginning from the parent directory of <paramref name="beginningDirectory"/>, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfDirectoryAbove(string directoryName, DirectoryInfo beginningDirectory, bool includeBeginningDirectory = false) =>
            GetDirectoryOfPathAbove((directoryInfo) => isDirectoryOfDirectoryAbove(directoryInfo, directoryName), beginningDirectory, includeBeginningDirectory);

        /// <summary>
        /// Get the absolute directory path of directory above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfDirectoryAbove(string directoryName, string directory, bool includeBeginningDirectory = false) =>
            GetDirectoryOfDirectoryAbove(directoryName, new DirectoryInfo(directory), includeBeginningDirectory: includeBeginningDirectory);

        /// <summary>
        /// Get the absolute directory path of directory above beginning from the parent directory of the entry point, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static DirectoryInfo GetDirectoryOfDirectoryAbove(string directoryName, bool includeBeginningDirectory = false) =>
            GetDirectoryOfDirectoryAbove(directoryName, AppDomain.CurrentDomain.BaseDirectory, includeBeginningDirectory);
    }
}
