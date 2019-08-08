using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IContainerUpdateSequenceStatus
    {
        bool IsContainerUpdating { get; }
        
        void BeginContainerUpdate();
        void EndContainerUpdate();
    }
}
