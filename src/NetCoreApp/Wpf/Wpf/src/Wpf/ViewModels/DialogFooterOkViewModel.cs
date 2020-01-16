using System.Windows.Input;
using Teronis.Models;
using Teronis.ViewModels;
using Teronis.Windows.Input;

namespace Teronis.Wpf.ViewModels
{
    public class DialogFooterOkViewModel : ViewModelBase, IDialogFooterViewModel
    {
        public bool? DialogResult { get; set; }
        public DialogButtons Buttons { get; private set; }
        public ICommand OkCommand { get; private set; }

        public DialogFooterOkViewModel(DialogButtons buttons)
        {
            Buttons = buttons;
            OkCommand = new CommandManagerRelayCommand<object>(onOkClicked);
        }

        private void onOkClicked(object parameter)
            => DialogResult = true;
    }
}
