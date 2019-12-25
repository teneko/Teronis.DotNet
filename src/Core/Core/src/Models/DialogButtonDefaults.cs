

namespace Teronis.Models
{
    public static class DialogButtonDefaults
    {
        public static DialogButtons DefaultButton;

        static DialogButtonDefaults()
            => DefaultButton = DialogButtons.Ok;
    }
}
