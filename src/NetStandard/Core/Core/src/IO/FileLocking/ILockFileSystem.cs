// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Teronis.IO.FileLocking
{
    public interface ILockFileSystem
    {
        FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);
    }
}
