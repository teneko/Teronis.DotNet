// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop
{
    public class SlimLazy<T>
    {
        /// <summary>
        /// True if the value has been created once.
        /// </summary>
        public bool IsValueCreated { get; private set; }

        /// <summary>
        /// Gets the value. If null it gets created.
        /// </summary>
        public T Value {
            get {
                if (!IsValueCreated) {
                    value = valueProvider!();
                    IsValueCreated = true;
                }

                return value;
            }
        }

        private readonly Func<T>? valueProvider;
        private T value;

        /// <summary>
        /// Constructs a instance of <see cref="SlimLazy{T}"/>.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        public SlimLazy(Func<T> valueProvider)
        {
            value = default!;
            this.valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        public SlimLazy(T value)
        {
            IsValueCreated = true;
            this.value = value;
        }
    }
}
