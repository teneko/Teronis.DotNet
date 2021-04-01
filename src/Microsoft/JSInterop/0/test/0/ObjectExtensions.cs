// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop
{
    public static class ObjectExtensions
    {
        public static object[] AsArray(this object value) =>
            new object[] { value };
    }
}
