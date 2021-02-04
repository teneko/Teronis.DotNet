namespace Teronis.Collections.Specialized
{
    public sealed class LinkedBucketListNode<KeyType, ValueType>
        where KeyType : notnull
    {
        public ValueType Value { get; }

        public LinkedBucketListNodePart ListPart { get; }
        public LinkedBucketListNodePart BucketPart { get; }
        public YetNullable<KeyType> Key => bucket.Key;

        public ILinkedBucketList<KeyType, ValueType>? Bucket => bucket;

        internal LinkedBucketListBase<KeyType, ValueType>.LinkedBucket bucket = null!;

        public LinkedBucketListNode(ValueType value)
        {
            ListPart = new LinkedBucketListNodeListPart(this);
            BucketPart = new LinkedBucketListNodeBucketPart(this);
            Value = value;
        }

        internal void Invalidate()
        {
            ListPart.Invalidate();
            bucket = null!;
            BucketPart.Invalidate();
        }

        public abstract class LinkedBucketListNodePart
        {
            public LinkedBucketListNode<KeyType, ValueType> Owner { get; } = null!;

            public LinkedBucketListNode<KeyType, ValueType>? Previous =>
                previous is null || Owner == head ? null : previous;

            public LinkedBucketListNode<KeyType, ValueType>? Next =>
                next is null || next == head ? null : next;

            public abstract LinkedBucketListNodePart? PreviousPart { get; }
            public abstract LinkedBucketListNodePart? NextPart { get; }

            internal LinkedBucketListNode<KeyType, ValueType> previous = null!;
            internal LinkedBucketListNode<KeyType, ValueType> next = null!;
            internal abstract LinkedBucketListNode<KeyType, ValueType>? head { get; }
            internal abstract LinkedBucketListNodePart previousPart { get; }
            internal abstract LinkedBucketListNodePart nextPart { get; }

            public LinkedBucketListNodePart(LinkedBucketListNode<KeyType, ValueType> owner) =>
                Owner = owner;

            public void Invalidate()
            {
                previous = null!;
                next = null!;
            }
        }

        internal class LinkedBucketListNodeListPart : LinkedBucketListNodePart
        {
            public override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart? PreviousPart => Previous?.ListPart;
            public override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart? NextPart => Next?.ListPart;

            internal override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart previousPart => previous.ListPart;
            internal override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart nextPart => next.ListPart;
            internal override LinkedBucketListNode<KeyType, ValueType>? head => Owner.bucket.list.head;

            public LinkedBucketListNodeListPart(LinkedBucketListNode<KeyType, ValueType> parted) : base(parted) { }
        }

        internal class LinkedBucketListNodeBucketPart : LinkedBucketListNodePart
        {
            public override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart? PreviousPart => Previous?.BucketPart;
            public override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart? NextPart => Next?.BucketPart;

            internal override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart previousPart => previous.BucketPart;
            internal override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart nextPart => next.BucketPart;
            internal override LinkedBucketListNode<KeyType, ValueType>? head => Owner.bucket.head;

            public LinkedBucketListNodeBucketPart(LinkedBucketListNode<KeyType, ValueType> parted) : base(parted) { }
        }
    }
}
