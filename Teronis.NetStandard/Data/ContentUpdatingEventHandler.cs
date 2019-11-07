using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void ContentUpdatingEventHandler<in ContentType>(object sender, IContentUpdatingEventArgs<ContentType> args);
}
