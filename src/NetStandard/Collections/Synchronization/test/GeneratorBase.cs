// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public abstract class GeneratorBase<T> : IEnumerable<object[]>
    {
        protected virtual T[] Values(params T[] values) =>
            values;

        protected object[] List(params object[] arrayOfValues) =>
            arrayOfValues;

        public abstract IEnumerator<object[]> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
