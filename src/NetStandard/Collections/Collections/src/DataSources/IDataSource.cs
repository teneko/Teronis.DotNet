using System;
using Teronis.Collections.DataSources.Generic;

namespace Teronis.Collections.DataSources
{
    public interface IDataSource : IDisposable
    {
        DataSourceEnumerationState EnumerationState { get; }

        /// <summary>
        /// Type of data this data source delivers.
        /// </summary>
        Type DataType { get; }

        public bool IsDisposed { get; }
    }
}
