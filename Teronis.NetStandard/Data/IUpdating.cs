using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IUpdateSequenceStatus
    {
        bool IsUpdating { get; }
        
        void BeginUpdate();
        void EndUpdate();
    }
}
