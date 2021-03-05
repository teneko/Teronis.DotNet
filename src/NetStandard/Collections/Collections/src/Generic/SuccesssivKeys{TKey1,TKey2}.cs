using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Teronis.Text;

namespace Teronis.Collections.Generic
{
    public readonly struct SuccessivKeys<TKey1, TKey2> : IOrderedKeysProvider
    {
        [MaybeNull, AllowNull]
        public readonly TKey1 Key1 { get; }
        [MaybeNull, AllowNull]
        public readonly TKey2 Key2 { get; }
        public readonly int KeysLength { get; }

        public SuccessivKeys([AllowNull] TKey1 key1, [AllowNull] TKey2 key2)
        {
            Key1 = key1;
            Key2 = key2;
            KeysLength = default;
            KeysLength = this.ThrowWhenNoSuccessivKey();
        }

        public void Deconstruct([MaybeNull] out TKey1 key1, [MaybeNull] out TKey2 key2)
        {
            key1 = Key1;
            key2 = Key2;
        }

        IList<object?> IOrderedKeysProvider.GetOrderedKeys() =>
            new object?[] { Key1, Key2 };

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var stringSeparationHelper = new StringSeparator(", ");
            stringBuilder.Append('[');

            foreach (var orderedKey in ((IOrderedKeysProvider)this).GetOrderedKeys()) {
                if (orderedKey != null) {
                    stringBuilder.Append(orderedKey.ToString());
                }

                stringSeparationHelper.SetSeparator(stringBuilder);
            }

            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
    }
}
