using MorseCode.ITask;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IContentUpdate
    {
        /// <summary>
        /// The source that created the update initially.
        /// </summary>
        object OriginalUpdateCreationSource { get; }
        object UpdateCreationSource { get; }
        object Content { get; }
        ITask<object> ContentTask { get; }
        bool IsContentTaskCompletedInitially { get; }
    }
}
