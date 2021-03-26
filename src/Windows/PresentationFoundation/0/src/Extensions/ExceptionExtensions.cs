// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Windows.PresentationFoundation.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ShowMessageBox(this Exception error)
            => MessageBoxUtils.ShowError(error);
    }
}
