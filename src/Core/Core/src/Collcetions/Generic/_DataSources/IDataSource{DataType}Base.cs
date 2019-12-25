using System;
using System.Collections.Generic;
using System.Threading;

namespace Teronis.Collections.Generic
{
    public interface IDataSourceBase<out DataType> : IDataSource { }
}
