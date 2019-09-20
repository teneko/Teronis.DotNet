using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IWorking
    {
        bool IsWorking { get; }
        
        void BeginWork();
        void EndWork();
    }
}
