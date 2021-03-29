// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Collections.Specialized;

namespace Teronis.Microsoft.JSInterop.Facades.PropertyAssigners
{
    public class PropertyAssignerFactories : OrderedDictionary<Type, Func<IServiceProvider, IPropertyAssigner>?>
    {
        public PropertyAssignerFactories()
        { }

        public PropertyAssignerFactories(IEqualityComparer<Type> comparer)
            : base(comparer) { }

        public PropertyAssignerFactories(IOrderedDictionary<Type, Func<IServiceProvider, IPropertyAssigner>?> dictionary)
            : base(dictionary) { }

        public PropertyAssignerFactories(IEnumerable<KeyValuePair<Type, Func<IServiceProvider, IPropertyAssigner>?>> items)
            : base(items) { }

        public PropertyAssignerFactories(IOrderedDictionary<Type, Func<IServiceProvider, IPropertyAssigner>?> dictionary, IEqualityComparer<Type> comparer)
            : base(dictionary, comparer) { }

        public PropertyAssignerFactories(IEnumerable<KeyValuePair<Type, Func<IServiceProvider, IPropertyAssigner>?>> items, IEqualityComparer<Type> comparer)
            : base(items, comparer) { }

        public void Add(Type implementationType) =>
            Add(implementationType, value: null);
    }
}
