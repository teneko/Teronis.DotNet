//using System;
//using Teronis.Threading.Tasks;

//namespace Teronis.Collections.Algorithms
//{
//    public class CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType> : EventArgs
//    {
//        public ICollectionModificationBundle<ConvertedItemType, CommonValueType> ConvertedCollectionChangeBundle { get; private set; }
//        public ICollectionModificationBundle<CommonValueType, object> OriginCollectionChangeBundle { get; private set; }

//        public CollectionChangeConversionAppliedEventArgs(IConvertedCollectionModificationBundle<ConvertedItemType, CommonValueType, object> bundles)
//        {
//            bundles = bundles ?? throw new ArgumentNullException(nameof(bundles));
//            ConvertedCollectionChangeBundle = bundles.ConvertedBundle;
//            OriginCollectionChangeBundle = bundles.OriginBundle;
//        }
//    }
//}
