

using Teronis.ObjectModel.Updates;

namespace Teronis.Data
{
    internal class UpdateWithTargetContainer<UpdateContentType, TargetType>
    {
        public ContentUpdate<UpdateContentType> Update { get; private set; }
        public TargetType Target { get; private set; }

        public UpdateWithTargetContainer(ContentUpdate<UpdateContentType> update, TargetType target)
        {
            Update = update;
            Target = target;
        }
    }
}
