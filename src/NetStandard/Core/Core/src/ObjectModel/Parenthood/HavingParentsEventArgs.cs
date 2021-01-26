using System;

namespace Teronis.ObjectModel.Parenthood
{
    public class HavingParentsEventArgs : EventArgs
    {
        public object OriginalSource { get; private set; }
        public ParentsContainer Container { get; private set; }

        public HavingParentsEventArgs(object originalSource, Type? wantedType)
        {
            OriginalSource = originalSource;
            Container = new ParentsContainer(wantedType);
        }
    }
}
