using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void UpdatedEventHandler<T>(object sender, Update<T> update);
}
