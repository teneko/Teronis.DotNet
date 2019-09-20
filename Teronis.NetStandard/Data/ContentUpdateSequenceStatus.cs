using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Teronis.Data
{
    public class WorkStatus : INotifyPropertyChanged, IWorking
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isUpdating;
        private int deepUpdateCounter;

        public bool IsWorking {
            get => isUpdating;

            private set {
                isUpdating = value;
                OnPropertyChanged();
            }
        }

        public WorkStatus() { }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void BeginWork()
        {
            if (deepUpdateCounter++ == 0) 
                IsWorking = true;

        }

        public void EndWork()
        {
            if (deepUpdateCounter == 0)
                throw new Exception("You cannot end the update before it has not begun");

            if (--deepUpdateCounter == 0)
                IsWorking = false;
        }
    }
}
