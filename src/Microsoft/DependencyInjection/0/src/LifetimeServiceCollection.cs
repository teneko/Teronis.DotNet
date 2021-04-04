// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public abstract class LifetimeServiceCollection<TDescriptor> : ILifetimeServiceCollection<TDescriptor>
        where TDescriptor : LifetimeServiceDescriptor
    {
        public abstract ServiceLifetime Lifetime { get; }

        public int Count =>
            Descriptors.Count;

        public bool IsReadOnly =>
            false;

        public virtual Type BaseType { get; }

        protected virtual List<TDescriptor> Descriptors { get; }

        public LifetimeServiceCollection(Type? baseType)
        {
            Descriptors = new List<TDescriptor>();
            BaseType = baseType ?? typeof(object);
        }

        public LifetimeServiceCollection()
            : this(baseType: null) { }

        protected void EnsureAssignableFromBaseType(Type serviceType)
        {
            if (!BaseType.IsAssignableFrom(serviceType)) {
                throw new ArgumentException($"Service type {serviceType} is not assignable from base type {BaseType}.");
            }
        }

        protected void EnsureAssignableFromBaseType(TDescriptor descriptor)
        {
            if (descriptor is null) {
                throw new ArgumentNullException(nameof(descriptor));
            }

            EnsureAssignableFromBaseType(descriptor.ServiceType);
        }

        public int IndexOf(TDescriptor descriptor) =>
            Descriptors.IndexOf(descriptor);

        public void Insert(int index, TDescriptor descriptor)
        {
            EnsureAssignableFromBaseType(descriptor);
            Descriptors.Insert(index, descriptor);
        }

        void ICollection<TDescriptor>.Add(TDescriptor descriptor)
        {
            EnsureAssignableFromBaseType(descriptor);
            Descriptors.Add(descriptor);
        }

        public void RemoveAt(int index) =>
            Descriptors.RemoveAt(index);

        public TDescriptor this[int index] {
            get => Descriptors[index];

            set {
                EnsureAssignableFromBaseType(value);
                Descriptors[index] = value;
            }
        }

        public void Clear() =>
            Descriptors.Clear();

        public bool Contains(TDescriptor descriptor) =>
            Descriptors.Contains(descriptor);

        public void CopyTo(TDescriptor[] array, int arrayIndex) =>
            throw new NotSupportedException();

        public bool Remove(TDescriptor descriptor) =>
            Descriptors.Remove(descriptor);

        public IEnumerator<TDescriptor> GetEnumerator()
        {
            foreach (var serviceDescriptor in Descriptors) {
                yield return serviceDescriptor;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
