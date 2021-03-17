using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    partial class ValueTaskType
    {
        internal class ThrowHelper
        {
            public static NotSupportedException CreateNotOfTypeValueTaskException(Type affectedType) =>
                new NotSupportedException($"The type {affectedType} is not {typeof(ValueTask)} or {typeof(ValueTask<>)}.");
        }
    }
}
