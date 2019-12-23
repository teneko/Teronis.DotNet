using System;

namespace Teronis.Data
{
    public class ContentUpdatedEventArgs<ContentType> : EventArgs, IContentUpdatedEventArgs<ContentType>
    {
        public IContentUpdate<ContentType> Update { get; private set; }

        public ContentUpdatedEventArgs(IContentUpdate<ContentType> update)
            => Update = update;
    }
}
