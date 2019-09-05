using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class Update<ContentType> : IUpdate<ContentType>
    {
        public object OriginalUpdateCreationSource { get; private set; }
        public object UpdateCreationSource { get; private set; }
        public ContentType Content { get; private set; }

        public Update(ContentType content, object originalUpdateCreationSource, object updateCreationSource)
        {
            Content = content;
            OriginalUpdateCreationSource = originalUpdateCreationSource;
            UpdateCreationSource = updateCreationSource;
        }

        public Update(ContentType content, object updateCreationSource)
            : this(content, updateCreationSource, updateCreationSource)
        { }
    }
}
