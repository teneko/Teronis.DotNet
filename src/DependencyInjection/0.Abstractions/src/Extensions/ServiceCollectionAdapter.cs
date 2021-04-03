// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection.Extensions
{
    internal class ServiceCollectionAdapter<TDescriptor, TCollection> : IServiceCollectionAdapter<TCollection>
        where TDescriptor : LifetimeServiceDescriptor
        where TCollection : ILifetimeServiceCollection<TDescriptor>
    {
        public TCollection LifetimeServiceCollection =>
            descriptorCollection;

        public int Count =>
            descriptorCollection.Count;

        public bool IsReadOnly =>
            descriptorCollection.IsReadOnly;

        private readonly TCollection descriptorCollection;
        private readonly DescriptorActivator<TDescriptor> descriptorActivator;

        public ServiceCollectionAdapter(TCollection descriptorCollection, DescriptorActivator<TDescriptor> descriptorActivator)
        {
            this.descriptorCollection = descriptorCollection;
            this.descriptorActivator = descriptorActivator;
        }

        public ServiceDescriptor this[int index] {
            get => descriptorCollection.CreateServiceDescriptor(descriptorCollection[index]);
            set => descriptorCollection[index] = descriptorActivator.CreateDescriptor(value);
        }

        public int IndexOf(ServiceDescriptor item) =>
            descriptorCollection.IndexOf(descriptorActivator.CreateDescriptor(item));

        public void Insert(int index, ServiceDescriptor item) =>
            descriptorCollection.Insert(index, descriptorActivator.CreateDescriptor(item));

        public void RemoveAt(int index) =>
            descriptorCollection.RemoveAt(index);

        public void Add(ServiceDescriptor item) =>
            descriptorCollection.Add(descriptorActivator.CreateDescriptor(item));

        public void Clear() =>
            descriptorCollection.Clear();

        public bool Contains(ServiceDescriptor item) =>
            descriptorCollection.Contains(descriptorActivator.CreateDescriptor(item));

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            var descriptors = descriptorCollection
                .Select(descriptor => descriptorCollection.CreateServiceDescriptor(descriptor));

            var list = new List<ServiceDescriptor>(descriptors);
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(ServiceDescriptor item) =>
            descriptorCollection.Remove(descriptorActivator.CreateDescriptor(item));

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            foreach (var lifetimeServiceDescriptor in descriptorCollection) {
                yield return descriptorCollection.CreateServiceDescriptor(lifetimeServiceDescriptor);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
