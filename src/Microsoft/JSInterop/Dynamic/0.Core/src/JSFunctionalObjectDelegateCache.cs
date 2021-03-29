// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    internal sealed class JSFunctionalObjectDelegateCache
    {
        public static JSFunctionalObjectDelegateCache Instance = new JSFunctionalObjectDelegateCache();

        public JSFunctionalObjectStaticDelegateCache.DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, object?[], object>> InvokeDelegateCache { get; }
        public JSFunctionalObjectStaticDelegateCache.DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, CancellationToken, object?[], object>> TokenCancellableInvokeDelegateCache { get; }
        public JSFunctionalObjectStaticDelegateCache.DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, TimeSpan, object?[], object>> TimeSpanCancellableInvokeDelegateCache { get; }

        private JSFunctionalObjectDelegateCache()
        {
            var jsFunctionalObjectType = typeof(IJSFunctionalObject);
            InvokeDelegateCache = new JSFunctionalObjectStaticDelegateCache.DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, object?[], object>>(jsFunctionalObjectType);
            TokenCancellableInvokeDelegateCache = new JSFunctionalObjectStaticDelegateCache.DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, CancellationToken, object?[], object>>(jsFunctionalObjectType);
            TimeSpanCancellableInvokeDelegateCache = new JSFunctionalObjectStaticDelegateCache.DelegateCache<Func<IJSFunctionalObject, IJSObjectReference, string, TimeSpan, object?[], object>>(jsFunctionalObjectType);
        }
    }
}
