// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Teronis.GitVersionCache.BuildTasks.Models;

namespace Teronis.GitVersionCache.BuildTasks
{
    public class GetVersionCacheTask : Task, ICacheIdentification
    {
        [Required]
        public string ProjectDirectory { get; set; }
        public string ConfigFile { get; set; }
        public bool NoFetch { get; set; }
        public bool NoNormalize { get; set; }

        [Required]
        public string CacheIdentifier { get; set; }

        [Output]
        public string LegacySemVerPadded { get; set; }
        [Output]
        public string VersionSourceSha { get; set; }
        [Output]
        public string CommitDate { get; set; }
        [Output]
        public string NuGetPreReleaseTag { get; set; }
        [Output]
        public string NuGetPreReleaseTagV2 { get; set; }
        [Output]
        public string NuGetVersion { get; set; }
        [Output]
        public string NuGetVersionV2 { get; set; }
        [Output]
        public string ShortSha { get; set; }
        [Output]
        public string Sha { get; set; }
        [Output]
        public string EscapedBranchName { get; set; }
        [Output]
        public string BranchName { get; set; }
        [Output]
        public string InformationalVersion { get; set; }
        [Output]
        public string FullSemVer { get; set; }
        [Output]
        public string AssemblySemFileVer { get; set; }
        [Output]
        public string AssemblySemVer { get; set; }
        [Output]
        public string CommitsSinceVersionSourcePadded { get; set; }
        [Output]
        public string LegacySemVer { get; set; }
        [Output]
        public string SemVer { get; set; }
        [Output]
        public string MajorMinorPatch { get; set; }
        [Output]
        public string FullBuildMetaData { get; set; }
        [Output]
        public string BuildMetaDataPadded { get; set; }
        [Output]
        public string BuildMetaData { get; set; }
        [Output]
        public string WeightedPreReleaseNumber { get; set; }
        [Output]
        public string PreReleaseNumber { get; set; }
        [Output]
        public string PreReleaseLabel { get; set; }
        [Output]
        public string PreReleaseTagWithDash { get; set; }
        [Output]
        public string PreReleaseTag { get; set; }
        [Output]
        public string Patch { get; set; }
        [Output]
        public string Minor { get; set; }
        [Output]
        public string Major { get; set; }
        [Output]
        public string CommitsSinceVersionSource { get; set; }

        public override bool Execute()
        {
            var taskTaskExecutor = new BuildTaskExecutor(this);
            var isCache = taskTaskExecutor.LoadCacheOrGetVersion(this);

            if (!isCache) {
                taskTaskExecutor.SaveToFilesystem(this);
            }

            return true;
        }
    }
}
