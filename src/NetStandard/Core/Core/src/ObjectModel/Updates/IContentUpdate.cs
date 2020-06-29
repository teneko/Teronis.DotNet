using MorseCode.ITask;

namespace Teronis.ObjectModel.Updates
{
    public interface IContentUpdate 
    {
        /// <summary>
        /// The source that created the update initially.
        /// </summary>
        object? OriginalUpdateCreationSource { get; }
        object? UpdateCreationSource { get; }
        ITask<object> ContentTask { get; }
        bool IsContentTaskCompleted { get; }
    }
}
