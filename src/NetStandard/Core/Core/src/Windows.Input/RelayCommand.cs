// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Input;

namespace Teronis.Windows.Input
{
    public class RelayCommand<T> : ICommand
    {
        private readonly RelayCommandExecutor<T> executeAction;
        private readonly RelayCommandPredicate<T>? canExecutePredicate;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="executeAction">The execution logic.</param>
        /// <param name="canExecutePredicate">The execution status logic.</param>
        public RelayCommand(RelayCommandExecutor<T> executeAction, RelayCommandPredicate<T>? canExecutePredicate)
        {
            this.executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            this.canExecutePredicate = canExecutePredicate;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeAction">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
        /// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
        public RelayCommand(RelayCommandExecutor<T> executeAction)
            : this(executeAction, null) { }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object? parameter) =>
            canExecutePredicate == null || canExecutePredicate((T)parameter);

        ///<summary>
        /// Occurs never and can be ignored if <see cref="canExecutePredicate"/> is null. It could for example reexposes RequerySuggested of the class of CommandManager.
        ///</summary>
#pragma warning disable 0067
        public virtual event EventHandler? CanExecuteChanged;
#pragma warning restore

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object? parameter) =>
            executeAction((T)parameter);
    }
}
