using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teronis.Data;
using Teronis.Models;

namespace Teronis.ViewModels.Wpf
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
