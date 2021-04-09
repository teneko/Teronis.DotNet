// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Input;
using Teronis.Windows.Input;

namespace Teronis.Windows.PresentationFoundation.Input
{
    public class CommandManagerRelayCommand<T> : RelayCommand<T>
    {
        public override event EventHandler? CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public CommandManagerRelayCommand(RelayCommandExecutor<T> executeAction, RelayCommandPredicate<T> canExecutePredicate)
            : base(executeAction, canExecutePredicate) { }

        public CommandManagerRelayCommand(RelayCommandExecutor<T> executeAction)
            : base(executeAction) { }
    }
}
