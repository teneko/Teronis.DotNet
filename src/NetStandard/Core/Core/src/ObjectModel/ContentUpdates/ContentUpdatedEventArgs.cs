using System;

namespace Teronis.ObjectModel.ContentUpdates
{
    public class ContentUpdatedEventArgs<ContentType> : EventArgs
    {
        public object ContentContainer { get; set; }
        public ContentType ContentUpdate { get; private set; }

        public ContentUpdatedEventArgs(object contentContainer, ContentType contentUpdate)
        {
            ContentContainer = contentContainer;
            ContentUpdate = contentUpdate;
        }
    }
}
