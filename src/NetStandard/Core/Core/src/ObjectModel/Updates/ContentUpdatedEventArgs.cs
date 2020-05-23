using System;

namespace Teronis.ObjectModel.Updates
{
    public class ContentUpdatedEventArgs<ContentType> : EventArgs, IContentUpdatedEventArgs<ContentType>
    {
        public IContentUpdate<ContentType> Update { get; private set; }

        public ContentUpdatedEventArgs(IContentUpdate<ContentType> update)
            => Update = update;
    }
}
