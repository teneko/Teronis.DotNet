using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class UpdatingEventArgs<T> : EventArgs
    {
        public Update<T> Update { get; private set; }
        public bool Handled { get; set; }

        public UpdatingEventArgs(Update<T> update)
            => Update = update;
    }
}
