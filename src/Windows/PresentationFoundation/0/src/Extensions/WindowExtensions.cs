using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Teronis.Windows.PresentationFoundation.Extensions
{
    public static class WindowExtensions
    {
        public static async Task<bool?> ShowDialogAsync(this Window window)
        {
            await Task.Yield();
            return window.ShowDialog();
        }

        public static bool IsModal(this Window window)
            => (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);

    }
}
