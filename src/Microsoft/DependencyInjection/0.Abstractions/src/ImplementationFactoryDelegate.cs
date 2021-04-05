// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection
{
    public delegate TResult ImplementationFactoryDelegate<in TProvider, out TResult>(TProvider provider)
        where TProvider : class, IServiceProvider;
}
