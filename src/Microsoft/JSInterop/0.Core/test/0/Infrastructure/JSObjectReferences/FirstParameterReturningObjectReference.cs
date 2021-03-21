using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Infrastructure.JSObjectReferences
{
    public class FirstParameterReturningObjectReference : JSEmptyObjectReference
    {
        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            if (args is null) {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length != 1) {
                throw new ArgumentException();
            }

            var argument = args[0];

            if (argument is null || argument.GetType() != typeof(TValue)) {
                throw new ArgumentException();
            }

            return new ValueTask<TValue>((TValue)argument);
        }
    }
}
