// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis
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
                    SetValueIfNotCreated(valueProvider!());
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

        /// <summary>
        /// Gets the value in the same way as <see cref="Value"/> do. 
        /// Typically it serves as a pointer for passing the method around.
        /// </summary>
        /// <returns></returns>
        public T GetValue() =>
            Value;

        /// <summary>
        /// Sets the value if it has been not created so far.
        /// </summary>
        /// <param name="value"></param>
        protected void SetValueIfNotCreated(T value)
        {
            if (IsValueCreated) {
                return;
            }

            this.value = value;
            IsValueCreated = true;
        }
    }
}
