//using Microsoft.Build.Framework;
//using GitVersion.MSBuildTask.Tasks;
//using System.ComponentModel;

//namespace Teronis.GitVersionCache.BuildTasks.Models
//{
//    public class SaveGetVersionTaskBase : GetVersion, IBuildIdentification, INotifyPropertyChanged
//    {
//        public event PropertyChangedEventHandler PropertyChanged;

//        public string CacheIdentifier { get; set; }
//        [Required]
//        public bool IsDateIdentifier { get; set; }

//        // ##################################
//        // # That what GitVersionTask would #
//        // # output we require as input for #
//        // # caching purposes.              #
//        // ##################################

//        [Required]
//        public new string Major {
//            get => base.Major;
//            set => base.Major = value;
//        }

//        [Required]
//        public new string Minor {
//            get => base.Minor;
//            set => base.Minor = value;
//        }

//        [Required]
//        public new string Patch {
//            get => base.Patch;
//            set => base.Patch = value;
//        }

//        [Required]
//        public new string PreReleaseTag {
//            get => base.PreReleaseTag;
//            set => base.PreReleaseTag = value;
//        }

//        [Required]
//        public new string PreReleaseTagWithDash {
//            get => base.PreReleaseTagWithDash;
//            set => base.PreReleaseTagWithDash = value;
//        }

//        [Required]
//        public new string PreReleaseLabel {
//            get => base.PreReleaseLabel;
//            set => base.PreReleaseLabel = value;
//        }

//        [Required]
//        public new string PreReleaseNumber {
//            get => base.PreReleaseNumber;
//            set => base.PreReleaseNumber = value;
//        }

//        [Required]
//        public new string WeightedPreReleaseNumber {
//            get => base.WeightedPreReleaseNumber;
//            set => base.WeightedPreReleaseNumber = value;
//        }

//        [Required]
//        public new string BuildMetaData {
//            get => base.BuildMetaData;
//            set => base.BuildMetaData = value;
//        }

//        [Required]
//        public new string BuildMetaDataPadded {
//            get => base.BuildMetaDataPadded;
//            set => base.BuildMetaDataPadded = value;
//        }

//        [Required]
//        public new string FullBuildMetaData {
//            get => base.FullBuildMetaData;
//            set => base.FullBuildMetaData = value;
//        }

//        [Required]
//        public new string MajorMinorPatch {
//            get => base.MajorMinorPatch;
//            set => base.MajorMinorPatch = value;
//        }

//        [Required]
//        public new string SemVer {
//            get => base.SemVer;
//            set => base.SemVer = value;
//        }

//        [Required]
//        public new string LegacySemVer {
//            get => base.LegacySemVer;
//            set => base.LegacySemVer = value;
//        }

//        [Required]
//        public new string LegacySemVerPadded {
//            get => base.LegacySemVerPadded;
//            set => base.LegacySemVerPadded = value;
//        }

//        [Required]
//        public new string AssemblySemVer {
//            get => base.AssemblySemVer;
//            set => base.AssemblySemVer = value;
//        }

//        [Required]
//        public new string AssemblySemFileVer {
//            get => base.AssemblySemFileVer;
//            set => base.AssemblySemFileVer = value;
//        }

//        [Required]
//        public new string FullSemVer {
//            get => base.FullSemVer;
//            set => base.FullSemVer = value;
//        }

//        [Required]
//        public new string InformationalVersion {
//            get => base.InformationalVersion;
//            set => base.InformationalVersion = value;
//        }

//        [Required]
//        public new string BranchName {
//            get => base.BranchName;
//            set => base.BranchName = value;
//        }

//        [Required]
//        public new string Sha {
//            get => base.Sha;
//            set => base.Sha = value;
//        }

//        [Required]
//        public new string ShortSha {
//            get => base.ShortSha;
//            set => base.ShortSha = value;
//        }

//        [Required]
//        public new string NuGetVersionV2 {
//            get => base.NuGetVersionV2;
//            set => base.NuGetVersionV2 = value;
//        }

//        [Required]
//        public new string NuGetVersion {
//            get => base.NuGetVersion;
//            set => base.NuGetVersion = value;
//        }

//        [Required]
//        public new string NuGetPreReleaseTagV2 {
//            get => base.NuGetPreReleaseTagV2;
//            set => base.NuGetPreReleaseTagV2 = value;
//        }

//        [Required]
//        public new string NuGetPreReleaseTag {
//            get => base.NuGetPreReleaseTag;
//            set => base.NuGetPreReleaseTag = value;
//        }

//        [Required]
//        public new string CommitDate {
//            get => base.CommitDate;
//            set => base.CommitDate = value;
//        }

//        [Required]
//        public new string VersionSourceSha {
//            get => base.VersionSourceSha;
//            set => base.VersionSourceSha = value;
//        }

//        [Required]
//        public new string CommitsSinceVersionSource {
//            get => base.CommitsSinceVersionSource;
//            set => base.CommitsSinceVersionSource = value;
//        }

//        [Required]
//        public new string CommitsSinceVersionSourcePadded {
//            get => base.CommitsSinceVersionSourcePadded;
//            set => base.CommitsSinceVersionSourcePadded = value;
//        }


//        protected override bool OnExecute() => true;

//        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
//        {
//            BuildTaskUtilities.SetUndefinedAsDefault(this, e.PropertyName, Log);
//            PropertyChanged?.Invoke(this, e);
//        }
//    }
//}
