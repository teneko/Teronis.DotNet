

namespace Teronis.Models
{
    public interface IDialogModel
    {
        string Message { get; }
        string Caption { get; }
        EDialogButtons Buttons { get; }
    }
}
