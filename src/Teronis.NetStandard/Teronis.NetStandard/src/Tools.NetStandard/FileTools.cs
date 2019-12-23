using System.IO;
using System.Threading.Tasks;

namespace Teronis.Tools.NetStandard
{
    public static class FileTools
    {
        public static async Task WriteAllBytes(string fileName, byte[] bytes)
        {
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bytes.Length, true)) {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
