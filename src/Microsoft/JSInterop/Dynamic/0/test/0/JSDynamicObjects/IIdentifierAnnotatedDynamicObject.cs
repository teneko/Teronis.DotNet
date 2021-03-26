// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IIdentifierAnnotatedDynamicObject : IJSObjectReferenceFacade
    {
        public const string javaScriptTypicalMethodName = nameof(javaScriptTypicalMethodName);

        [Identifier(javaScriptTypicalMethodName)]
        public ValueTask<string> CSharpTypicalMethodNameAsync();
    }
}
