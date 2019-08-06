
namespace Teronis.Models.NetStandard
{
    public class DialogModel : IDialogModel
    {
        public string Message { get; private set; }
        public string Caption { get; private set; }

        public DialogModel(string message, string caption) {
            Message = message;
            Caption = caption;
        }

        public DialogModel(string message)
            : this(message, default) { }
    }
}
