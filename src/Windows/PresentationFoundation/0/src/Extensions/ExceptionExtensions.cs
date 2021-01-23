using System;

namespace Teronis.Windows.PresentationFoundation.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ShowMessageBox(this Exception error)
            => MessageBoxUtils.ShowError(error);
    }
}
