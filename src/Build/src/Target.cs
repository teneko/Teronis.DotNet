using Teronis.Build.CommandOptions;

namespace Teronis.Build
{
    public sealed class Target
    {
        public static Target Restore = RestoreCommandOptions.RestoreCommand;
        public static Target RestoreAndBuildSynthetic = BuildSyntheticCommandOptions.BuildSyntheticCommand;
        public static Target RestoreAndBuildSyntheticAndNonSynthethic = BuildCommandOptions.BuildCommand;
        public static Target RestoreAndBuildAndPack = PackCommandOptions.PackCommand;
        public static Target RestoreAndBuildAndTest = TestCommandOptions.TestCommand;
        public static Target RestoreAndBuildAndPackAndTest = AzureCommandOptions.AzureCommand;

        public string Name { get; }

        public Target(string name) =>
            Name = name ?? throw new System.ArgumentNullException(nameof(name));

        public static implicit operator Target(string name) =>
            new Target(name);

        public static implicit operator string(Target target) =>
            target.Name;
    }
}
