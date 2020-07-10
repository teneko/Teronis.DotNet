using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MorseCode.ITask;

namespace Teronis.ObjectModel.Updates
{
    public class ContentUpdate<ContentType> : IContentUpdate<ContentType>
    {
        public object? OriginalUpdateCreationSource { get; private set; }
        public object? UpdateCreationSource { get; private set; }

        public ITask<ContentType> ContentTask { get; private set; } = null!;
        public bool IsContentTaskCompleted => contentTask.IsCompleted;

        Task<ContentType> contentTask = null!;

        /// <summary>
        /// </summary>
        /// <param name="content">The parameter <paramref name="content"/> will be wrapped by <see cref="Task.FromResult{TResult}(TResult)"/>.</param>
        /// <param name="originalUpdateCreationSource"></param>
        /// <param name="updateCreationSource"></param>
        public ContentUpdate([AllowNull] ContentType content, object? originalUpdateCreationSource, object? updateCreationSource)
        {
            var contentTask = Task.FromResult(content!);
            setContentTask(contentTask);
            onConstruction(originalUpdateCreationSource, updateCreationSource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">The parameter <paramref name="content"/> will be wrapped by <see cref="Task.FromResult{TResult}(TResult)"/>.</param>
        /// <param name="updateCreationSource"></param>
        public ContentUpdate(ContentType content, object updateCreationSource)
            : this(content, updateCreationSource, updateCreationSource)
        { }

        public ContentUpdate(Task<ContentType> contentTask, object? originalUpdateCreationSource, object? updateCreationSource)
        {
            setContentTask(contentTask);
            onConstruction(originalUpdateCreationSource, updateCreationSource);
        }

        public ContentUpdate(Task<ContentType> contentTask, object? updateCreationSource)
            : this(contentTask, updateCreationSource, updateCreationSource)
        { }

        public ContentUpdate(ITask<ContentType> contentTaskInterface, object? originalUpdateCreationSource, object? updateCreationSource)
        {
            var contentTask = contentTaskInterface.AsTask();
            setContentTask(contentTask, contentTaskInterface: contentTaskInterface);
            onConstruction(originalUpdateCreationSource, updateCreationSource);
        }

        public ContentUpdate(ITask<ContentType> contentTaskInterface, object? updateCreationSource)
            : this(contentTaskInterface, updateCreationSource, updateCreationSource)
        { }

        private void onConstruction(object? originalUpdateCreationSource, object? updateCreationSource)
        {
            OriginalUpdateCreationSource = originalUpdateCreationSource;
            UpdateCreationSource = updateCreationSource;
        }

        private void setContentTask(Task<ContentType> contentTask, ITask<ContentType>? contentTaskInterface = null)
        {
            ContentTask = contentTaskInterface ?? contentTask.AsITask();
            this.contentTask = contentTask;
        }

        #region IContentUpdate

        ITask<object> IContentUpdate.ContentTask => (ITask<object>)ContentTask;

        #endregion
    }
}
