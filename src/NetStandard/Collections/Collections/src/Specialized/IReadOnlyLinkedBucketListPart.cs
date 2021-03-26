// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Specialized
{
    public interface IReadOnlyLinkedBucketListNode<KeyType, out ValueType>
        where KeyType : notnull
    {
        ValueType Value { get; }

        IReadOnlyLinkedBucketListNodePart ListPart { get; }
        IReadOnlyLinkedBucketListNodePart BucketPart { get; }
        IYetNullable<KeyType> Key { get; }

        IReadOnlyLinkedBucketList<KeyType, ValueType>? Bucket { get; }

        public interface IReadOnlyLinkedBucketListNodePart
        {
            IReadOnlyLinkedBucketListNode<KeyType, ValueType> Owner { get; }
            IReadOnlyLinkedBucketListNode<KeyType, ValueType>? Previous { get; }
            IReadOnlyLinkedBucketListNode<KeyType, ValueType>? Next { get; }
            IReadOnlyLinkedBucketListNode<KeyType, ValueType>.IReadOnlyLinkedBucketListNodePart? PreviousPart { get; }
            IReadOnlyLinkedBucketListNode<KeyType, ValueType>.IReadOnlyLinkedBucketListNodePart? NextPart { get; }
        }
    }
}
