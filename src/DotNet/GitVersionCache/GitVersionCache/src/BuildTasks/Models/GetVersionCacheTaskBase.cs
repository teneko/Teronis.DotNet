using Microsoft.Build.Framework;
using GitVersion.MSBuildTask.Tasks;

namespace Teronis.DotNet.GitVersionCache.BuildTasks.Models
{
    public abstract class GetVersionCacheTaskBase : GetVersion, IBuildIdentification
    {
        [Required]
        public string BuildIdentifier { get; set; }
        [Required]
        public bool IsDateIdentifier { get; set; }
    }
}
