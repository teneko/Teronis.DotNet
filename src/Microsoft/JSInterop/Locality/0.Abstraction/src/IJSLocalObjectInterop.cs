using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public interface IJSLocalObjectInterop
    {
        /// <summary>
        /// Creates an object reference. If <paramref name="objectName"/> is null
        /// or equals "window" then an object reference to window gets returned.
        /// </summary>
        /// <param name="objectName">The object name.</param>
        /// <returns>A JavaScript object reference.</returns>
        ValueTask<IJSObjectReference> GetGlobalObjectReference(string? objectName);
        ValueTask<IJSObjectReference> GetLocalObjectReference(IJSObjectReference objectReference, string objectName);
    }
}
