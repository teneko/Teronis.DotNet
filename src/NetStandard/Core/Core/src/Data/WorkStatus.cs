using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Teronis.Data
{
    public class WorkStatus : INotifyPropertyChanged, IWorking
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isWorking;
        private int deepWorkCounter;

        public bool IsWorking {
            get => isWorking;

            private set {
                isWorking = value;
                OnPropertyChanged();
            }
        }

        public WorkStatus() { }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void BeginWork()
        {
            if (deepWorkCounter++ == 0) {
                IsWorking = true;
            }

        }

        public void EndWork()
        {
            if (deepWorkCounter == 0) {
                throw new Exception("You cannot end the update before it has not begun");
            }

            if (--deepWorkCounter == 0) {
                IsWorking = false;
            }
        }
    }
}
