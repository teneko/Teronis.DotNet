using System.Runtime.CompilerServices;

namespace Teronis.ObjectModel
{
    public interface IMemberCallablePropertyNotificationComponent
    {
        void OnPropertyChanged([CallerMemberName] string? propertyName = null);
        void OnPropertyChanging([CallerMemberName] string? propertyName = null);
    }
}