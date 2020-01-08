using System.ComponentModel;
using Teronis.Models;

namespace Teronis.ViewModels.Wpf
{
    public interface IDialogFooterViewModel : INotifyPropertyChanged
    {
        bool? DialogResult { get; set; }
        DialogButtons Buttons { get; }
    }
}
