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
