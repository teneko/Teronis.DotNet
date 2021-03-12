using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IIdentifierAnnotatedDynamicObject : IJSDynamicObject
    {
        public const string javaScriptTypicalMethodName = nameof(javaScriptTypicalMethodName);

        [Identifier(javaScriptTypicalMethodName)]
        public ValueTask<string> CSharpTypicalMethodNameAsync();
    }
}
