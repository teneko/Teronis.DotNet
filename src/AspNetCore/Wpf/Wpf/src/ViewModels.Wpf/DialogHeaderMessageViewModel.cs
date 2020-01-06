using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teronis.ViewModels.Wpf
{
    public class DialogHeaderMessageViewModel : ViewModelBase, IDialogHeaderViewModel
    {
        public string Message { get; private set; }

        public DialogHeaderMessageViewModel(string message)
            => Message = message;
    }
}
