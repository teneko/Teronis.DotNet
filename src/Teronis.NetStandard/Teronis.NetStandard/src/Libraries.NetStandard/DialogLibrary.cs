using Teronis.Models;

namespace Teronis.Libraries.NetStandard
{
    public static class DialogLibrary
    {
        public static EDialogButtons DefaultButton;

        static DialogLibrary()
            => DefaultButton = EDialogButtons.Ok;
    }
}
