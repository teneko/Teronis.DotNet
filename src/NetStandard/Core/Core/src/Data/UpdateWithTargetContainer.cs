

using Teronis.ObjectModel.Updates;

namespace Teronis.Data
{
    internal class UpdateWithTargetContainer<UpdateContentType, TargetType>
    {
            public ContentUpdate<UpdateContentType> Update { get; set; }
            public TargetType Target { get; set; }
    }
}
