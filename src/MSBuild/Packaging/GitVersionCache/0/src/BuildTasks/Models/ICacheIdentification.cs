// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.GitVersionCache.BuildTasks.Models
{
    public interface ICacheIdentification
    {
        string CacheIdentifier { get; }
        string ProjectDirectory { get; }
        string ConfigFile { get; }
    }
}
