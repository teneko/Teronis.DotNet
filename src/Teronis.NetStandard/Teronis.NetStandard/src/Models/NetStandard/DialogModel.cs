
using Teronis.Libraries.NetStandard;

namespace Teronis.Models.NetStandard
{
    public class DialogModel : IDialogModel
    {
        public string Message { get; private set; }
        public string Caption { get; private set; }
        public EDialogButtons Buttons { get; private set; }

        public DialogModel(string message, string caption, EDialogButtons buttons)
        {
            Message = message;
            Caption = caption;
            Buttons = buttons;
        }

        public DialogModel(string message, string caption)
            : this(message, caption, DialogLibrary.DefaultButton) { }

        public DialogModel(string message, EDialogButtons buttons)
            : this(message, null, buttons) { }

        public DialogModel(string message)
            : this(message, null, DialogLibrary.DefaultButton) { }
    }
}
