using System;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public static class IJSObjectReferenceFacadeExtensions
    {
        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, params object?[] arguments) =>
            jsObjectReferenceFacade.InvokeAsync<TValue>(identifier, arguments);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, CancellationToken cancellationToken, params object?[] arguments) =>
            jsObjectReferenceFacade.InvokeAsync<TValue>(identifier, cancellationToken, arguments);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, TimeSpan timeout, params object?[] arguments) =>
            jsObjectReferenceFacade.InvokeAsync<TValue>(identifier, timeout, arguments);

        public static ValueTask InvokeVoidAsync(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, params object?[] arguments) =>
            jsObjectReferenceFacade.InvokeVoidAsync(identifier, arguments);

        public static ValueTask InvokeVoidAsync(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, CancellationToken cancellationToken, params object?[] arguments) =>
            jsObjectReferenceFacade.InvokeVoidAsync(identifier, cancellationToken, arguments);

        public static ValueTask InvokeVoidAsync(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, TimeSpan timeout, params object?[] arguments) =>
            jsObjectReferenceFacade.InvokeVoidAsync(identifier, timeout, arguments);
    }
}
