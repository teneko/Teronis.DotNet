// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Teronis.IO
{
    public static class FileUtils
    {
        /// <summary>
        /// Gets the absolute file path of file above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static FileInfo? GetPathOfFileAbove(string fileNameWithExtension, DirectoryInfo directory, bool includeBeginningDirectory = false)
        {
            var foundDirectory = DirectoryUtils.GetDirectoryOfFileAbove(fileNameWithExtension, directory, includeBeginningDirectory);

            if (foundDirectory != null) {
                return new FileInfo(Path.Combine(foundDirectory.FullName, fileNameWithExtension));
            } else {
                return null;
            }
        }

        /// <summary>
        /// Gets the absolute file path of file above beginning from the parent directory of <paramref name="directory"/>, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static FileInfo? GetPathOfFileAbove(string fileNameWithExtension, string directory, bool includeBeginningDirectory = false)
        {
            directory = directory ?? throw new ArgumentNullException(directory);
            return GetPathOfFileAbove(fileNameWithExtension, new DirectoryInfo(directory), includeBeginningDirectory);
        }

        /// <summary>
        /// Gets the absolute file path of file above beginning from the parent directory of the entry point, unless <paramref name="includeBeginningDirectory"/> is true.
        /// </summary>
        public static FileInfo? GetPathOfFileAbove(string fileNameWithExtension, bool includeBeginningDirectory = false) =>
            GetPathOfFileAbove(fileNameWithExtension, AppDomain.CurrentDomain.BaseDirectory!, includeBeginningDirectory);
    }
}
