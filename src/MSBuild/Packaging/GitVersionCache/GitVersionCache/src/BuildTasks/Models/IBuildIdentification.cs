using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.GitVersionCache.BuildTasks.Models
{
    public interface IBuildIdentification
    {
        string CacheIdentifier { get; set; }
        bool IsDateIdentifier { get; set; }
    }
}
