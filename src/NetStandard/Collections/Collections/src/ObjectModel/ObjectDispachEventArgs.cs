// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Collections.ObjectModel
{
    public class ObjectDispachEventArgs<ObjectType> : EventArgs
    {
        public ObjectType Object { get; private set; }

        public ObjectDispachEventArgs(ObjectType @object)
            => Object = @object;
    }
}
