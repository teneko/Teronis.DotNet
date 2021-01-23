using System.IO;

namespace Teronis.Build
{
    public class ProjectInfo
    {
        public FileInfo FileInfo { get; }
        public string Name => FileInfo.Name;
        public string Path => FileInfo.FullName;

        public ProjectInfo(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public override string ToString() =>
            Name;
    }
}
