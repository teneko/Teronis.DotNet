using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void UpdatingEventHandler<in ContentType>(object sender, IUpdatingEventArgs<ContentType> args);
}
