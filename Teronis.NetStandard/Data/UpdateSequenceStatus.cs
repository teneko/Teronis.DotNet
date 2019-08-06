using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Teronis.Data
{
    public class UpdateSequenceStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isUpdating;
        private bool isExplicitUpdateBegin;

        public bool IsUpdating {
            get => isUpdating;

            private set {
                isUpdating = value;
                OnPropertyChanged();
            }
        }

        public UpdateSequenceStatus() { }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// The intention is to avoid the possible exception of <see cref="BeginUpdate"/>.
        /// </summary>
        public void BeginUpdate(bool isImplicitUpdateBegin)
        {
            if (isImplicitUpdateBegin && IsUpdating)
                return;
            if (IsUpdating)
                throw new Exception("Update process has been already started. Did you forgot to end the update?");

            IsUpdating = true;
            isExplicitUpdateBegin = !isImplicitUpdateBegin;
        }

        public void BeginUpdate()
            => BeginUpdate(false);

        private void endUpdate()
            => IsUpdating = false;

        public void EndUpdate(bool isImplicitUpdateEnd)
        {
            if (!IsUpdating)
                throw new Exception("You cannot end the update before it has not begun");

            if (isImplicitUpdateEnd && isExplicitUpdateBegin)
                return;
            else if ((isImplicitUpdateEnd && !isExplicitUpdateBegin) || isExplicitUpdateBegin)
                endUpdate();
            else // if (!isImplicitUpdateEnd)
                return; // user have to end update explicit
        }

        public void EndUpdate()
            => EndUpdate(false);
    }
}
