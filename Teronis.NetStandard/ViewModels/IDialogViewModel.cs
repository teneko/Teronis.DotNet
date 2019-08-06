
namespace Teronis.ViewModels
{
    public interface IDialogViewModel
    {
        string Message { get; }
        string Caption { get; }
        bool? DialogResult { get; }
    }
}
