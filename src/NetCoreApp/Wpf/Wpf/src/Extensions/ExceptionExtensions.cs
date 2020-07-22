using System;
using Teronis.Windows;

namespace Teronis.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ShowMessageBox(this Exception error)
            => MessageBoxUtils.ShowError(error);
    }
}
