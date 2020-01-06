using System.Windows;
using System.Windows.Input;
using Teronis.Models;
using Teronis.Windows.Input.Wpf;

namespace Teronis.ViewModels.Wpf
{
    public class DialogFooterOkViewModel : ViewModelBase, IDialogFooterViewModel
    {
        public bool? DialogResult { get; set; }
        public DialogButtons Buttons { get; private set; }
        public ICommand OkCommand { get; private set; }

        public DialogFooterOkViewModel(DialogButtons buttons)
        {
            Buttons = buttons;
            OkCommand = new RelayCommand<object>(onOkClicked);
        }

        private void onOkClicked(object parameter)
            => DialogResult = true;
    }
}
