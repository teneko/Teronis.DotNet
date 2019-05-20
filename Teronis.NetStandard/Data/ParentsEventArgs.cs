using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Teronis.Data
{
    public class ParentsEventArgs : EventArgs
    {
        public ParentsContainer Container { get; private set; }

        public ParentsEventArgs(Type wantedType) => Container = new ParentsContainer(wantedType);
    }
}
