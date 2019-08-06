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
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        public event UpdatingEventHandler<T> Updating;
        public event UpdatedEventHandler<T> Updated;

        public bool IsUpdating => updateSequenceStatus.IsUpdating;

        private UpdateSequenceStatus updateSequenceStatus;
        private PropertyChangedRelay propertyChangedRelay;

        public Updater()
        {
            updateSequenceStatus = new UpdateSequenceStatus();
            propertyChangedRelay = new PropertyChangedRelay(GetType(), updateSequenceStatus);
            propertyChangedRelay.PropertyChanged += PropertyChangedRelay_PropertyChanged;
        }

        private void PropertyChangedRelay_PropertyChanged(object sender, PropertyChangedEventArgs e)
           => PropertyChanged?.Invoke(this, e);

        public bool IsUpdatable(Update<T> update)
           => new UpdatingEventArgs<T>(update).IsUpdateAppliable(this, Updating);

        /// <summary>
        /// When using this function, you have to call <see cref="EndUpdate"/> by yourself explicit.
        /// </summary>
        public void BeginUpdate()
            => updateSequenceStatus.BeginUpdate();

        /// <summary>
        /// Buisness logic have to be implemented here
        /// </summary>
        /// <param name="update"></param>
        protected virtual void InnerUpdatBy(Update<T> update) { }

        public void EndUpdate()
            => updateSequenceStatus.EndUpdate();

        public void UpdateBy(Update<T> update)
        {
            if (IsUpdatable(update)) {
                updateSequenceStatus.BeginUpdate(true);
                InnerUpdatBy(update);
                Updated?.Invoke(this, update);
                updateSequenceStatus.EndUpdate(true);
            }
        }
    }
}
