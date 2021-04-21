// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Models
{
    public static class DialogButtonDefaults
    {
        public static DialogButtons DefaultButton;

        static DialogButtonDefaults()
            => DefaultButton = DialogButtons.Ok;
    }
}
