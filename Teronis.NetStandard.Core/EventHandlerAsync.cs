using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xhobot
{
    public delegate Task EventHandlerAsync<in TSender, in TArgs>(TSender sender, TArgs eventArgs);
}
