using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public interface IChild<T> where T : IChild<T>
    {
        T TryGetSubLayer();
    }
}
