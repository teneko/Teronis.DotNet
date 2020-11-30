using System.Windows.Input;
using Teronis.Models;
using Teronis.ViewModels;
using Teronis.Windows.PresentationFoundation.Input;

namespace Teronis.Windows.PresentationFoundation.ViewModels
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
            YesCommand = new CommandManagerRelayCommand<object>(onYesClicked);
            NoCommand = new CommandManagerRelayCommand<object>(onNoClicked);
        }

        private void onYesClicked(object parameter)
            => DialogResult = true;

        private void onNoClicked(object parameter)
            => DialogResult = false;
    }
}
