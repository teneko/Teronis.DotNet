// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Teronis.Models;

namespace Teronis.Windows.PresentationFoundation.ViewModels
{
    public interface IDialogFooterViewModel : INotifyPropertyChanged
    {
        bool? DialogResult { get; set; }
        DialogButtons Buttons { get; }
    }
}
