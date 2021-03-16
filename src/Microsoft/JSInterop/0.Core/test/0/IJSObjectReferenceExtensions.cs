using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Infrastructure.JSObjectReferences;

namespace Teronis.Microsoft.JSInterop
{
    internal static class IJSObjectReferenceExtensions
    {
        public static ValueTask InvokeVoidAsync(this IJSFunctionalObject jsFunctionalObject) =>
            jsFunctionalObject.InvokeVoidAsync(new JSEmptyObjectReference(), string.Empty, new object[0]);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSFunctionalObject jsFunctionalObject) =>
            jsFunctionalObject.InvokeAsync<TValue>(new JSEmptyObjectReference(), string.Empty, new object[0]);
    }
}
