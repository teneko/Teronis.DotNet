using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Teronis.IO;

namespace Teronis.DotNet.Build
{
    public static class Utilities
    {
        public static DirectoryInfo GetRootDirectory() 
            => DirectoryTools.GetDirectoryOfFileAbove(".msbuild", AppDomain.CurrentDomain.BaseDirectory);
    }
}
