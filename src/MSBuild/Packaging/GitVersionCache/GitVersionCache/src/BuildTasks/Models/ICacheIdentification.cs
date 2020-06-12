using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.GitVersionCache.BuildTasks.Models
{
    public interface ICacheIdentification
    {
        string CacheIdentifier { get; }
        string ProjectDirectory { get; }
    }
}
