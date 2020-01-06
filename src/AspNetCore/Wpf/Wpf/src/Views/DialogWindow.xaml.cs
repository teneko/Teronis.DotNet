using System;
using System.Windows;
using Teronis.Models;
using Teronis.ViewModels;
using Teronis.ViewModels.Wpf;

namespace Teronis.Views
{
    public partial class DialogWindow : Window
    {
        public static bool? ShowDialog(Window owner, IDialogModel model)
        {
            var headerViewModel = new DialogHeaderMessageViewModel(model.Message);
            IDialogFooterViewModel footerViewModel;

            if (model.Buttons == DialogButtons.YesNo)
                footerViewModel = new DialogFooterYesNoViewModel(model.Buttons);
            else if (model.Buttons == DialogButtons.Ok)
                footerViewModel = new DialogFooterOkViewModel(model.Buttons);
            else
                throw new NotImplementedException("This type of buttons is not implemented yet");

            var dialogViewModel = new DialogViewModel(model.Caption,
                headerViewModel,
                footerViewModel);

            var dialog = new DialogWindow()
            {
                DataContext = dialogViewModel,
                Owner = owner
            };

            return dialog.ShowDialog();
        }

        public DialogWindow()
            => InitializeComponent();
    }
}
