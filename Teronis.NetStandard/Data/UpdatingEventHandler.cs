using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void UpdatingEventHandler<T>(object sender, UpdatingEventArgs<T> args);
}
