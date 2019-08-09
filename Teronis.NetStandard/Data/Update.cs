using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class Update<ContentType> : IUpdate<ContentType>
    {
        public object UpdateCreationSource { get; private set; }
        public ContentType Content { get; private set; }

        public Update(ContentType content, object updateCreationSource)
        {
            Content = content;
            UpdateCreationSource = updateCreationSource;
        }
    }
}
