using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class UpdatingEventArgs<ContentType> : EventArgs, IUpdatingEventArgs<ContentType>
    {
        public IUpdate<ContentType> Update { get; private set; }
        public bool Handled { get; set; }

        public UpdatingEventArgs(IUpdate<ContentType> update)
            => Update = update;
    }
}
