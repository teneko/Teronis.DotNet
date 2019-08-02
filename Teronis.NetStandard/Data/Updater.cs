using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using PropertyChanging;
using Teronis.Extensions.NetStandard;
using Teronis.Tools.NetStandard;

namespace Teronis.Data
{
    public class Updater<T> : INotifyPropertyChanged, IUpdatable<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event UpdatingEventHandler<T> Updating;
        public event UpdatedEventHandler<T> Updated;

        public bool IsUpdating {
            get => isUpdating;

            private set {
                isUpdating = value;
                OnPropertyChanged();
            }
        }

        private bool isUpdating;

        public bool IsUpdatable(Update<T> update)
           => new UpdatingEventArgs<T>(update).IsUpdateAppliable(this, Updating);

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void BeginUpdate()
            => IsUpdating = true;

        /// <summary>
        /// Buisness logic have to be implemented here
        /// </summary>
        /// <param name="update"></param>
        protected virtual void InnerUpdatBy(Update<T> update) { }

        public void EndUpdate()
            => IsUpdating = false;

        public void UpdateBy(Update<T> update)
        {
            EndUpdate();

            if (!IsUpdatable(update)) {
                EndUpdate();
                return;
            }

            InnerUpdatBy(update);
            EndUpdate();
            Updated?.Invoke(this, update);
        }
    }
}
