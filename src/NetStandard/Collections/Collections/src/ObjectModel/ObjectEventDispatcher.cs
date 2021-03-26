// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Collections.ObjectModel
{
    public class ObjectEventDispatcher<ObjectType> : IDisposable
    {
        public event EventHandler<ObjectDispachEventArgs<ObjectType>>? ObjectDispatch;

        private bool isDisposed;

        public void DispatchObject(ObjectType @object)
        {
            var args = new ObjectDispachEventArgs<ObjectType>(@object);
            ObjectDispatch?.Invoke(this, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) {
                return;
            }

            ObjectDispatch = null;
            isDisposed = true;
        }

        ~ObjectEventDispatcher() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
