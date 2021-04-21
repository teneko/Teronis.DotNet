// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Utils
{
    public static class SystemParametersUtils
    {
        public static int CalculateKeyboardDelayInterval(int keyboardDelay)
            => (keyboardDelay + 1) * 250;
    }
}
