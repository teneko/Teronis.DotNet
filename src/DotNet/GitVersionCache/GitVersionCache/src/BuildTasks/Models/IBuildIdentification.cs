using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.DotNet.GitVersionCache.BuildTasks.Models
{
    public interface IBuildIdentification
    {
        string BuildIdentifier { get; set; }
        bool IsDateIdentifier { get; set; }
    }
}
