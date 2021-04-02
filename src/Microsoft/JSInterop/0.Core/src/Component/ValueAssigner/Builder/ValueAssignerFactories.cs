// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Collections.Specialized;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder
{
    public class ValueAssignerFactories : OrderedDictionary<Type, Func<IServiceProvider, IValueAssigner>?>
    {
        public ValueAssignerFactories()
        { }

        public ValueAssignerFactories(IEqualityComparer<Type> comparer)
            : base(comparer) { }

        public ValueAssignerFactories(IOrderedDictionary<Type, Func<IServiceProvider, IValueAssigner>?> dictionary)
            : base(dictionary) { }

        public ValueAssignerFactories(IEnumerable<KeyValuePair<Type, Func<IServiceProvider, IValueAssigner>?>> items)
            : base(items) { }

        public ValueAssignerFactories(IOrderedDictionary<Type, Func<IServiceProvider, IValueAssigner>?> dictionary, IEqualityComparer<Type> comparer)
            : base(dictionary, comparer) { }

        public ValueAssignerFactories(IEnumerable<KeyValuePair<Type, Func<IServiceProvider, IValueAssigner>?>> items, IEqualityComparer<Type> comparer)
            : base(items, comparer) { }

        public void Add(Type implementationType) =>
            Add(implementationType, value: null);
    }
}
