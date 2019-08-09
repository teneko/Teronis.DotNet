using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IUpdatingEventArgs<out ContentType>
    {
        IUpdate<ContentType> Update { get; }
        bool Handled { get; set; }
    }
}
