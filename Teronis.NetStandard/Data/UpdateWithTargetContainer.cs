using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    internal class UpdateWithTargetContainer<UpdateContentType, TargetType>
    {
            public Update<UpdateContentType> Update { get; set; }
            public TargetType Target { get; set; }
    }
}
