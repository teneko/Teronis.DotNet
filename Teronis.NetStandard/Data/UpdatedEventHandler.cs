using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void UpdatedEventHandler<in ContentType>(object sender, IUpdate<ContentType> update);
}
