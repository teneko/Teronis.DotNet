// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Input;
using Teronis.Models;
using Teronis.ViewModels;
using Teronis.Windows.PresentationFoundation.Input;

namespace Teronis.Windows.PresentationFoundation.ViewModels
{
    public class DialogFooterYesNoViewModel : ViewModelBase, IDialogFooterViewModel
    {
        public bool? DialogResult {
            get => dialogResult;
            set => PropertyChangeComponent.ChangeProperty(ref dialogResult, value);
        }

        public DialogButtons Buttons { get; private set; }
        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        private bool? dialogResult;

        public DialogFooterYesNoViewModel(DialogButtons buttons)
        {
            Buttons = buttons;
            YesCommand = new CommandManagerRelayCommand<object>(OnClickedYes);
            NoCommand = new CommandManagerRelayCommand<object>(OnClickedNo);
        }

        private void OnClickedYes(object? _) =>
            DialogResult = true;

        private void OnClickedNo(object? _) =>
            DialogResult = false;
    }
}
