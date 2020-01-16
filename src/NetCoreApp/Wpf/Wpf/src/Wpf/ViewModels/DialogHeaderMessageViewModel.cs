using Teronis.ViewModels;

namespace Teronis.Wpf.ViewModels
{
    public class DialogHeaderMessageViewModel : ViewModelBase, IDialogHeaderViewModel
    {
        public string Message { get; private set; }

        public DialogHeaderMessageViewModel(string message)
            => Message = message;
    }
}
