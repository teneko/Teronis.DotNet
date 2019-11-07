using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Teronis.Tools.NetStandard
{
    public static class AssemblyTools
    {
        /// <summary>
        /// Get the text file of an embedded resource as string.
        /// </summary>
        /// <param name="fileName">The full path including namespace and folder.</param>
        public static string GetEmbeddedResourceTextFile(Assembly assembly, string fileName)
        {
            using (var stream = assembly.GetManifestResourceStream(fileName)) {
                using (var reader = new StreamReader(stream)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
