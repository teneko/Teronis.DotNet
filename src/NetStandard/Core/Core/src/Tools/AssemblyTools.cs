using System;
using System.IO;
using System.Reflection;

namespace Teronis.Tools
{
    public static class AssemblyTools
    {
        /// <summary>
        /// Get the text file of an embedded resource as string.
        /// </summary>
        /// <param name="fileName">The full path including namespace and folder.</param>
        public static string GetEmbeddedResourceTextFile(Assembly assembly, string fileName)
        {
            using var stream = assembly.GetManifestResourceStream(fileName) ??
                throw new ArgumentException("The embedded file name does not exist.");

            // Read to end from stream.
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
