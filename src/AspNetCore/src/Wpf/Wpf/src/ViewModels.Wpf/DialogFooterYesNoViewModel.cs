using System.Windows;
using System.Windows.Input;
using Teronis.Models;
using Teronis.Windows.Input.Wpf;

namespace Teronis.ViewModels.Wpf
{
    public class DialogFooterYesNoViewModel : ViewModelBase, IDialogFooterViewModel
    {
        public bool? DialogResult { get; set; }
        public DialogButtons Buttons { get; private set; }
        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        public DialogFooterYesNoViewModel(DialogButtons buttons)
        {
            Buttons = buttons;
            YesCommand = new RelayCommand<object>(onYesClicked);
            NoCommand = new RelayCommand<object>(onNoClicked);
        }

        private void onYesClicked(object parameter)
            => DialogResult = true;

        private void onNoClicked(object parameter)
            => DialogResult = false;
    }
}
