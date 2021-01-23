using System;
using System.Windows.Input;
using Teronis.Windows.Input;

namespace Teronis.Windows.PresentationFoundation.Input
{
    public class CommandManagerRelayCommand<T> : RelayCommand<T>
    {
        public override event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public CommandManagerRelayCommand(Action<T> executeAction, Predicate<T> canExecutePredicate)
            : base(executeAction, canExecutePredicate) { }

        public CommandManagerRelayCommand(Action<T> executeAction)
            : base(executeAction) { }
    }
}
