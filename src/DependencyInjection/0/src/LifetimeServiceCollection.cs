// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public abstract class LifetimeServiceCollection<TDescriptor> : ILifetimeServiceCollection<TDescriptor>
    {
        public abstract ServiceLifetime Lifetime { get; }

        public int Count =>
            descriptors.Count;

        public bool IsReadOnly =>
            false;

        private readonly List<TDescriptor> descriptors;

        public LifetimeServiceCollection() =>
            descriptors = new List<TDescriptor>();

        public int IndexOf(TDescriptor descriptor) =>
            descriptors.IndexOf(descriptor);

        public void Insert(int index, TDescriptor descriptor) =>
            descriptors.Insert(index, descriptor);

        void ICollection<TDescriptor>.Add(TDescriptor descriptor) =>
            descriptors.Add(descriptor);

        public void RemoveAt(int index) =>
            descriptors.RemoveAt(index);

        public TDescriptor this[int index] {
            get => descriptors[index];
            set => descriptors[index] = value;
        }

        public void Clear() =>
            descriptors.Clear();

        public bool Contains(TDescriptor descriptor) =>
            descriptors.Contains(descriptor);

        public void CopyTo(TDescriptor[] array, int arrayIndex) =>
            throw new NotSupportedException();

        public bool Remove(TDescriptor descriptor) =>
            descriptors.Remove(descriptor);

        public IEnumerator<TDescriptor> GetEnumerator()
        {
            foreach (var serviceDescriptor in descriptors) {
                yield return serviceDescriptor;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
