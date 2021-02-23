using System.ComponentModel;
using System.Runtime.CompilerServices;
using Teronis.ObjectModel;

namespace Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections
{
    public abstract class CustomSynchronizingCollection<SuperItemType, SubItemType> : SynchronizingCollectionBase<SuperItemType, SubItemType>, INotifyPropertyChanged, INotifyPropertyChanging
        where SubItemType : notnull
        where SuperItemType : notnull
    {
        protected PropertyNotificationComponent PropertyNotificationComponent { get; }

        public CustomSynchronizingCollection() =>
            PropertyNotificationComponent = new PropertyNotificationComponent();

        protected void OnPropertyChanging([CallerMemberName] string? propertyName = null) =>
            PropertyNotificationComponent.OnPropertyChanged(propertyName);

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyNotificationComponent.OnPropertyChanged(propertyName);

        public event PropertyChangedEventHandler PropertyChanged {
            add => ((INotifyPropertyChanged)PropertyNotificationComponent).PropertyChanged += value;
            remove => ((INotifyPropertyChanged)PropertyNotificationComponent).PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler PropertyChanging {
            add => ((INotifyPropertyChanging)PropertyNotificationComponent).PropertyChanging += value;
            remove => ((INotifyPropertyChanging)PropertyNotificationComponent).PropertyChanging -= value;
        }
    }
}
