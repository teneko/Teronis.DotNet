using System.ComponentModel;
using Teronis.Models;

namespace Teronis.Wpf.ViewModels
{
    public interface IDialogFooterViewModel : INotifyPropertyChanged
    {
        bool? DialogResult { get; set; }
        DialogButtons Buttons { get; }
    }
}
