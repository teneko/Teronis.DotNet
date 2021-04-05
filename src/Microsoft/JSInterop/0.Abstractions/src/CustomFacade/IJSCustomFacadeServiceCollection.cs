// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public interface IJSCustomFacadeServiceCollection : ILifetimeServiceCollection<JSCustomFacadeServiceDescriptor>
    {
        IJSCustomFacadeServiceCollection UseExtension(Action<ScopedServicesExtension<IAsyncDisposable, IJSCustomFacadeFactoryServiceProvider, JSCustomFacadeServiceDescriptor, IJSCustomFacadeServiceCollection>> callback);
    }
}
