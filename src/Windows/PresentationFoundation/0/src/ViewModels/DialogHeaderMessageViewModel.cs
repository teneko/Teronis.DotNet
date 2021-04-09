// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.ViewModels;

namespace Teronis.Windows.PresentationFoundation.ViewModels
{
    public class DialogHeaderMessageViewModel : ViewModelBase, IDialogHeaderViewModel
    {
        public string Message { get; private set; }

        public DialogHeaderMessageViewModel(string message) => 
            Message = message;
    }
}
