using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Teronis.Windows.Input;

namespace Teronis.Windows.Input.Wpf
{
    public class RelayCommand<T> : Input.RelayCommand<T>
    {
        public override event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<T> executeAction, Predicate<T> canExecutePredicate)
            : base(executeAction, canExecutePredicate) { }

        public RelayCommand(Action<T> executeAction)
            : base(executeAction) { }
    }
}
