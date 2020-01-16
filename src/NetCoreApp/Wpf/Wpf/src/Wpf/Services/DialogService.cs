using System.Threading.Tasks;
using System.Windows;
using Teronis.Models;
using Teronis.Services;
using Teronis.Windows.Controls;

namespace Teronis.Wpf.Services
{
    public class DialogService : IAsyncDialogService
    {
        public Window Owner { get; private set; }

        public DialogService(Window owner)
            => Owner = owner;

        public Task<bool?> ShowDialogAsync(IDialogModel dialogModel)
            => Task.FromResult(DialogWindow.ShowDialog(Owner, dialogModel));
    }
}
