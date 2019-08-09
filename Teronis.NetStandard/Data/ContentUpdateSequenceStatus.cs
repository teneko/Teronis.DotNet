using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Teronis.Data
{
    public class ContainerUpdateSequenceStatus : INotifyPropertyChanged, IContentUpdateSequenceStatus
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isUpdating;
        private int deepUpdateCounter;

        public bool IsContentUpdating {
            get => isUpdating;

            private set {
                isUpdating = value;
                OnPropertyChanged();
            }
        }

        public ContainerUpdateSequenceStatus() { }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void BeginContentUpdate()
        {
            if (deepUpdateCounter++ == 0) 
                IsContentUpdating = true;

        }

        public void EndContentUpdate()
        {
            if (deepUpdateCounter == 0)
                throw new Exception("You cannot end the update before it has not begun");

            if (--deepUpdateCounter == 0)
                IsContentUpdating = false;
        }
    }
}
