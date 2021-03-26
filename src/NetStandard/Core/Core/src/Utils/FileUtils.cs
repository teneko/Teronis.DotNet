// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading.Tasks;

namespace Teronis.Utils
{
    public static class FileUtils
    {
        public static async Task WriteAllBytes(string fileName, byte[] bytes)
        {
            using var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bytes.Length, true);
            await fs.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
