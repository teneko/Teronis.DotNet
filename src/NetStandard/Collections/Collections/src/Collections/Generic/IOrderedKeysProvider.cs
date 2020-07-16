using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface IOrderedKeysProvider
    {
        public int KeysLength { get; }

        public IList<object?> GetOrderedKeys();
    }
}
