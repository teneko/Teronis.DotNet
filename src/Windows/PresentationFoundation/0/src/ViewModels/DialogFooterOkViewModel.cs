// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Input;
using Teronis.Models;
using Teronis.ViewModels;
using Teronis.Windows.PresentationFoundation.Input;

namespace Teronis.Windows.PresentationFoundation.ViewModels
{
    public class DialogFooterOkViewModel : ViewModelBase, IDialogFooterViewModel
    {
        public bool? DialogResult {
            get => dialogResult;
            set => PropertyChangeComponent.ChangeProperty(ref dialogResult, value);
        }

        public DialogButtons Buttons { get; private set; }
        public ICommand OkCommand { get; private set; }

        private bool? dialogResult;

        public DialogFooterOkViewModel(DialogButtons buttons)
        {
            Buttons = buttons;
            OkCommand = new CommandManagerRelayCommand<object>(OnClickedOk);
        }

        private void OnClickedOk(object? _) =>
            DialogResult = true;
    }
}
