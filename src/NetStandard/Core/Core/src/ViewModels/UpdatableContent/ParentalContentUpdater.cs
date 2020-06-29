using System;
using Teronis.Data;
using Teronis.ObjectModel.Updates;

namespace Teronis.ViewModels.UpdatableContent
{
    public abstract class ParentalContentUpdater<ParentType, ContentType> : ContentUpdater<ContentType>
        where ParentType : notnull
    {
        protected ParentType parent { get; private set; } = default!;

        public ParentalContentUpdater(WorkStatus workStatus, ParentType parent)
            : base(workStatus)
            => onConstruction(parent);

        private void onConstruction(ParentType parent)
            => this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
    }
}
