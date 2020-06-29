


namespace Teronis.Models
{
    public class DialogModel : IDialogModel
    {
        public string Message { get; private set; }
        public string? Caption { get; private set; }
        public DialogButtons Buttons { get; private set; }

        public DialogModel(string message, string? caption, DialogButtons buttons)
        {
            Message = message;
            Caption = caption;
            Buttons = buttons;
        }

        public DialogModel(string message, string? caption)
            : this(message, caption, DialogButtonDefaults.DefaultButton) { }

        public DialogModel(string message, DialogButtons buttons)
            : this(message, null, buttons) { }

        public DialogModel(string message)
            : this(message, null, DialogButtonDefaults.DefaultButton) { }
    }
}
