

namespace Teronis.Models
{
    public interface IDialogModel
    {
        string Message { get; }
        string Caption { get; }
        DialogButtons Buttons { get; }
    }
}
