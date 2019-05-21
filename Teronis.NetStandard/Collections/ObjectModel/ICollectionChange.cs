using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Collections.ObjectModel
{
    public interface ICollectionChange<out T>
    {
        NotifyCollectionChangedAction ChangeAction { get; }
        T OldValue { get; }
        int OldIndex { get; }
        T NewValue { get; }
        int NewIndex { get; }
    }
}
