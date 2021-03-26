// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



namespace Teronis.Collections.DataSources.Generic
{
    public enum DataSourceEnumerationState
    {
        Enumerable = 1,
        Started = 2,
        Stopped = 4,
        Completed = 8 | Stopped,
        Faulted = 16 | Stopped,
    }
}
