using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teronis.Localization.NetStandard;
using Teronis.Models;
using Teronis.Models.NetStandard;
using Teronis.Services;

namespace Teronis.Extensions.NetStandard
{
    public static class IAsyncDialogServiceExtensions
    {
        public static Task<bool?> ShowErrorDialogAsync(this IAsyncDialogService dialogService, Exception error)
        {
            var caption = StringResources.AnErrorOccuredExclamation;
            DialogModel dialogModel;
#if DEBUG
            dialogModel = new DialogModel(error.ToString(), caption);
#else
            dialogModel = new DialogModel(error.Message, caption);
#endif
            return dialogService.ShowDialogAsync(dialogModel);
        }
    }
}
