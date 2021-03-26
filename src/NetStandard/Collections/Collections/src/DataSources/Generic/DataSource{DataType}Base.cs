// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


/* Nicht gemergte Änderung aus Projekt "GitVersionCache.NetStandard.Core (netstandard2.0)"
Vor:
using Microsoft.Extensions.Logging;
using System;
using Teronis.Collections.DataSources;
Nach:
using System;
using Microsoft.Extensions.Logging;
using Teronis.Collections.DataSources;
*/

/* Nicht gemergte Änderung aus Projekt "Teronis.NetStandard.Core (netcoreapp3.1)"
Vor:
using Microsoft.Extensions.Logging;
using System;
using Teronis.Collections.DataSources;
Nach:
using System;
using Microsoft.Extensions.Logging;
using Teronis.Collections.DataSources;
*/

/* Nicht gemergte Änderung aus Projekt "GitVersionCache.NetStandard.Core (netcoreapp3.1)"
Vor:
using Microsoft.Extensions.Logging;
using System;
using Teronis.Collections.DataSources;
Nach:
using System;
using Microsoft.Extensions.Logging;
using Teronis.Collections.DataSources;
*/
using System;
using Microsoft.Extensions.Logging;

namespace Teronis.Collections.DataSources.Generic
{
    public abstract class DataSourceBase<TData> : IDataSource
    {
        public bool IsDisposed { get; private set; }
        public DataSourceEnumerationState EnumerationState { get; protected set; }
        public Type DataType { get; private set; }

        protected ILogger? Logger { get; private set; }

        public DataSourceBase(ILogger? logger = null)
        {
            EnumerationState = DataSourceEnumerationState.Enumerable;
            DataType = typeof(TData);
            Logger = logger;
        }

        /// <summary>
        /// Throws exception if <see cref="IsDisposed"/> is true or <see cref="EnumerationState"/> is not equals <see cref="DataSourceEnumerationState.Enumerable"/>.
        /// </summary>
        protected void EnsureEnumerate()
        {
            if (IsDisposed) {
                throw new ObjectDisposedException("The data source has been disposed and can not be enumerated again");
            }

            if (!EnumerationState.HasFlag(DataSourceEnumerationState.Enumerable)) {
                throw new InvalidOperationException("The data source has been already enumerated");
            }
        }

        protected void LogEnumerationReachedEnd()
        {
            Logger?.LogDebug($"Enumeration of type {DataType.Name} reached end ({nameof(EnumerationState)}={EnumerationState})");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) {
                return;
            }

            IsDisposed = true;

            // Dispose managed resources
            if (disposing) { }
        }
    }
}
