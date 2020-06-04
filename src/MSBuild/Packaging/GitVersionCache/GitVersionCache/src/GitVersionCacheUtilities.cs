using Microsoft.Build.Utilities;

namespace Teronis.GitVersionCache
{
    public class GitVersionCacheUtilities
    {
        public static int GetTaskMaxDepth<T>(T derivedTask)
        where T : Task
        {
            var derivedTaskType = derivedTask.GetType();
            var objectType = typeof(object);
            var taskType = typeof(Task);
            var depth = 0;

            while (derivedTaskType != taskType) {
                derivedTaskType = derivedTaskType.BaseType;
                depth++;

                if (derivedTaskType == objectType) {
                    return 0;
                }
            }

            return depth;
        }
    }
}
