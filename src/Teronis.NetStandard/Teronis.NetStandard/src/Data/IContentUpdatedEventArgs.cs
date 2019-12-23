using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IContentUpdatedEventArgs<out ContentType>
    {
        IContentUpdate<ContentType> Update { get; }
    }
}
