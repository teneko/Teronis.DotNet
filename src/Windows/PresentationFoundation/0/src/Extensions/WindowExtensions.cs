// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Teronis.Windows.PresentationFoundation.Extensions
{
    public static class WindowExtensions
    {
        /// <summary>
        /// Shows the dialog after calling <see cref="Task.Yield"/>.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static async Task<bool?> ShowDialogAsync(this Window window)
        {
            await Task.Yield();
            return window.ShowDialog();
        }

        public static bool IsModal(this Window window) =>
            (bool)(typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(window)
                ?? throw new NotImplementedException("The Window's internal field '_showingAsDialog' is not implemented."));

    }
}
