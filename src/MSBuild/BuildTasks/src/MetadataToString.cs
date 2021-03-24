using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Teronis.MSBuild.BuildTasks
{
    /// <summary>
    /// First level metadata are transformed to semicolon separated key value pairs.
    /// </summary>
    public class MetadataToString : Task
    {
        [Required]
        public ITaskItem[] Item { get; set; } = null!;

        [Output]
        public string? Metadata { get; set; }

        public override bool Execute()
        {
            StringBuilder command = new StringBuilder();

            if (Item is null || Item.Length == 0) {
                Log.LogError($"Property '{nameof(Item)}' is empty.");
                return false;
            }

            if (Item.Length > 1) {
                Log.LogError($"Property '{nameof(Item)}' has more than one items.");
                return false;
            }

            var item = Item[0];

            foreach (object? parameterObject in item.MetadataNames) {
                if (parameterObject is string parameter) {
                    command.AppendFormat("{0}={1};", parameter, item.GetMetadata(parameter));
                }
            }

            Metadata = command.ToString();
            return true;
        }
    }
}
