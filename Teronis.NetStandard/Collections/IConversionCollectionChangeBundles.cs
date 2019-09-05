using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public interface IConversionCollectionChangeBundles<ConvertedItemType, CommonValueType, OriginContentType>
    {
        ICollectionChangeBundle<ConvertedItemType, CommonValueType> ConvertedBundle { get; }
        ICollectionChangeBundle<CommonValueType, OriginContentType> OriginBundle { get; }
    }
}
