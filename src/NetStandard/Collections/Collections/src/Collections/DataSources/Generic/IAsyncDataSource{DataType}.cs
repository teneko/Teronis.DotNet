using System.Collections.Generic;
using System.Threading;

namespace Teronis.Collections.DataSources.Generic
{
    public interface IAsyncDataSource<out DataType> : IDataSource<DataType>
    {
        IAsyncEnumerable<DataType> EnumerateAsync(CancellationToken cancellationToken = default);
    }
}
