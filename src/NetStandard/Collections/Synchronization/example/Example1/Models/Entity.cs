using System.ComponentModel;

namespace Teronis.Collections.Synchronization.Example1.Models
{
    public class Entity : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler? PropertyChanged;
    }
}
