using System.ComponentModel;

namespace Teronis.ObjectModel
{
    public interface IEventInvocablePropertyNotificationComponent
    {
        void OnPropertyChanged(object? sender, PropertyChangedEventArgs args);
        void OnPropertyChanging(object? sender, PropertyChangingEventArgs args);
    }
}