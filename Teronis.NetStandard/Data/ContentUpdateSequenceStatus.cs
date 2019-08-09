using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Teronis.Data
{
    public class ContainerUpdateSequenceStatus : INotifyPropertyChanged, IContainerUpdateSequenceStatus
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isUpdating;
        private int deepUpdateCounter;

        public bool IsContainerUpdating {
            get => isUpdating;

            private set {
                isUpdating = value;
                OnPropertyChanged();
            }
        }

        public ContainerUpdateSequenceStatus() { }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void BeginContainerUpdate()
        {
            if (deepUpdateCounter++ == 0) 
                IsContainerUpdating = true;

        }

        public void EndContainerUpdate()
        {
            if (deepUpdateCounter == 0)
                throw new Exception("You cannot end the update before it has not begun");

            if (--deepUpdateCounter == 0)
                IsContainerUpdating = false;
        }
    }
}
