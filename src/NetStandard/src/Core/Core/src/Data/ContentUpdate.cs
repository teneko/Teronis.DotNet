using MorseCode.ITask;
using System;
using System.Threading.Tasks;

namespace Teronis.Data
{
    public class ContentUpdate<ContentType> : IContentUpdate<ContentType>
    {
        public object OriginalUpdateCreationSource { get; private set; }
        public object UpdateCreationSource { get; private set; }

        public ContentType Content
            => IsContentTaskCompleted
                ? (contentTask.Status == TaskStatus.RanToCompletion
                    ? contentTask.Result
                    : throw contentTask.Exception)
                : throw new InvalidOperationException("You cannot get the result before it has not been computed");

        public ITask<ContentType> ContentTask { get; private set; }
        public bool IsContentTaskCompletedInitially { get; private set; }
        public bool IsContentTaskCompleted => contentTask.IsCompleted;

        Task<ContentType> contentTask;

        public ContentUpdate(ContentType content, object originalUpdateCreationSource, object updateCreationSource)
        {
            var contentTask = Task.FromResult(content);
            setContentTask(contentTask);
            onConstruction(originalUpdateCreationSource, updateCreationSource);
        }

        public ContentUpdate(ContentType content, object updateCreationSource)
            : this(content, updateCreationSource, updateCreationSource)
        { }

        public ContentUpdate(Task<ContentType> contentTask, object originalUpdateCreationSource, object updateCreationSource)
        {
            setContentTask(contentTask);
            onConstruction(originalUpdateCreationSource, updateCreationSource);
        }

        public ContentUpdate(Task<ContentType> contentTask, object updateCreationSource)
            : this(contentTask, updateCreationSource, updateCreationSource)
        { }

        public ContentUpdate(ITask<ContentType> contentTask, object originalUpdateCreationSource, object updateCreationSource)
            : this(contentTask.AsTask(), originalUpdateCreationSource, updateCreationSource)
        { }

        public ContentUpdate(ITask<ContentType> contentTask, object updateCreationSource)
            : this(contentTask, updateCreationSource, updateCreationSource)
        { }

        private void onConstruction(object originalUpdateCreationSource, object updateCreationSource)
        {
            OriginalUpdateCreationSource = originalUpdateCreationSource;
            UpdateCreationSource = updateCreationSource;
        }

        private void setContentTask(Task<ContentType> contentTask)
        {
            ContentTask = contentTask.AsITask();
            IsContentTaskCompletedInitially = contentTask.IsCompleted;
            this.contentTask = contentTask;
        }

#region IContentUpdate

        object IContentUpdate.Content => Content;
        ITask<object> IContentUpdate.ContentTask => (ITask<object>)ContentTask;

#endregion
    }
}
