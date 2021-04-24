// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Teronis.ComponentModel;

namespace Teronis.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged {
            add => PropertyChangeComponent.PropertyChanged += value;
            remove => PropertyChangeComponent.PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler? PropertyChanging {
            add => PropertyChangeComponent.PropertyChanging += value;
            remove => PropertyChangeComponent.PropertyChanging -= value;
        }

        protected PropertyChangeComponent PropertyChangeComponent { get; private set; } = null!;

        public ViewModelBase()
        {
            PropertyChangeComponent = new PropertyChangeComponent(this);
        }
    }
}
