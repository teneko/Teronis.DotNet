// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Models;

namespace Teronis.Services
{
    public interface IAsyncDialogService
    {
        Task<bool?> ShowDialogAsync(IDialogModel dialogModel);
    }
}
