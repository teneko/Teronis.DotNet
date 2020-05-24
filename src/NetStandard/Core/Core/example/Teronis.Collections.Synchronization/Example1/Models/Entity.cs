using System.ComponentModel;

namespace Teronis.Collections.Synchronization.Example1.Models
{
    public class Entity : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public virtual event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
    }
}
