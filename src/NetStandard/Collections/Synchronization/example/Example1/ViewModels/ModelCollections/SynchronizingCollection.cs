using System.ComponentModel;
using System.Runtime.CompilerServices;
using Teronis.ObjectModel;

namespace Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections
{
    public abstract class CustomSynchronizingCollection<SuperItemType, SubItemType> : SynchronizingCollectionBase<SuperItemType, SubItemType>, INotifyPropertyChanged, INotifyPropertyChanging
        where SubItemType : notnull
        where SuperItemType : notnull
    {
        public event PropertyChangedEventHandler PropertyChanged {
            add => PropertyChangeComponent.PropertyChanged += value;
            remove => PropertyChangeComponent.PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler PropertyChanging {
            add => PropertyChangeComponent.PropertyChanging += value;
            remove => PropertyChangeComponent.PropertyChanging -= value;
        }

        protected PropertyChangeComponent PropertyChangeComponent { get; }

        public CustomSynchronizingCollection() =>
            PropertyChangeComponent = new PropertyChangeComponent();

        protected void OnPropertyChanging([CallerMemberName] string? propertyName = null) =>
            PropertyChangeComponent.OnPropertyChanged(propertyName);

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChangeComponent.OnPropertyChanged(propertyName);
    }
}
