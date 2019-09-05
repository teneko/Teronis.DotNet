
using Teronis.Models;

namespace Teronis.ViewModels
{
    public interface IDialogViewModel
    {
        string Message { get; }
        string Caption { get; }
        EDialogButtons Buttons { get; }
        bool? DialogResult { get; }
    }
}
