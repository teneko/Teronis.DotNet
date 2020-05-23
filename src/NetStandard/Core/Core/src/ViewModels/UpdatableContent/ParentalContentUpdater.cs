using Teronis.Data;
using Teronis.ObjectModel.Updates;

namespace Teronis.ViewModels.UpdatableContent
{
    public abstract class ParentalContentUpdater<ParentType, ContentType> : ContentUpdater<ContentType>
    {
        protected ParentType parent { get; private set; }

        public ParentalContentUpdater(WorkStatus workStatus, ParentType parent)
            : base(workStatus)
            => onConstruction(parent);

        private void onConstruction(ParentType parent)
            => this.parent = parent;
    }
}
