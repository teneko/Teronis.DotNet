using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IContentUpdateSequenceStatus
    {
        bool IsContentUpdating { get; }
        
        void BeginContentUpdate();
        void EndContentUpdate();
    }
}
