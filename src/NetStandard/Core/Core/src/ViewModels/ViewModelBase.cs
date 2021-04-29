// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Threading;
using Teronis.ComponentModel;

namespace Teronis.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged {
            add => ChangeComponent.PropertyChanged += value;
            remove => ChangeComponent.PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler? PropertyChanging {
            add => ChangeComponent.PropertyChanging += value;
            remove => ChangeComponent.PropertyChanging -= value;
        }

        protected virtual PropertyChangeComponent ChangeComponent {
            get {
                if (changeComponent is null) {
                    Interlocked.CompareExchange(ref changeComponent!, new PropertyChangeComponent(this), null!);
                }

                return changeComponent;
            }
        }

        private PropertyChangeComponent? changeComponent;
    }
}
