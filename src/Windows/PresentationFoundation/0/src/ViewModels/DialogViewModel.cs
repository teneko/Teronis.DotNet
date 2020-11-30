using Teronis.ViewModels;

namespace Teronis.Windows.PresentationFoundation.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        public string Caption { get; private set; }
        public IDialogHeaderViewModel HeaderViewModel { get; private set; }
        public IDialogFooterViewModel FooterViewModel { get; private set; }

        public DialogViewModel(string caption, IDialogHeaderViewModel headerViewModel, IDialogFooterViewModel footerViewModel)
        {
            Caption = caption;
            HeaderViewModel = headerViewModel;
            FooterViewModel = footerViewModel;
        }
    }
}
