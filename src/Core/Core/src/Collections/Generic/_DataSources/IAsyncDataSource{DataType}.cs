using System.Collections.Generic;
using System.Threading;

namespace Teronis.Collections.Generic
{
    public interface IAsyncDataSource<out DataType> : IDataSourceBase<DataType>
    {
        IAsyncEnumerable<DataType> EnumerateAsync(CancellationToken cancellationToken = default);
    }
}
