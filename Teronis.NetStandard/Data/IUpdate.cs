using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IUpdate<out ContentType>
    {
        object OriginalUpdateCreationSource { get; }
        ContentType Content { get; }
    }
}
