using System;

namespace Teronis.ObjectModel.Updates
{
    public class ContentUpdatingEventArgs<ContentType> : EventArgs, IContentUpdatingEventArgs<ContentType>
    {
        public IContentUpdate<ContentType> Update { get; private set; }
        public bool Handled { get; set; }

        public ContentUpdatingEventArgs(IContentUpdate<ContentType> update)
            => Update = update;
    }
}
