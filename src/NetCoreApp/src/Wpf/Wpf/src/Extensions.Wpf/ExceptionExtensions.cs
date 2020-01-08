using System;
using Teronis.Tools.Wpf;

namespace Teronis.Extensions.Wpf
{
    public static class ExceptionExtensions
    {
        public static void ShowMessageBox(this Exception error)
            => MessageBoxTools.ShowError(error);
    }
}
