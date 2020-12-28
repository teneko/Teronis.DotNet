using System;

namespace Teronis.ObjectModel.ContentUpdates
{
    public class ContentUpdatingEventArgs<ContentType> : EventArgs
    {
        public object ContentContainer { get; set; }
        public ContentType ContentUpdate { get; private set; }
        public bool Handled { get; set; }

        public ContentUpdatingEventArgs(object contentContainer, ContentType contentUpdate)
        {
            ContentContainer = contentContainer;
            ContentUpdate = contentUpdate;
        }
    }
}
