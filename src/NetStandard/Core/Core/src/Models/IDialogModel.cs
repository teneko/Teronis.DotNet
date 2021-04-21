// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Models
{
    public interface IDialogModel
    {
        string Message { get; }
        string? Caption { get; }
        DialogButtons Buttons { get; }
    }
}
