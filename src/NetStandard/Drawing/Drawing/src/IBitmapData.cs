// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;

namespace Teronis.Drawing
{
    public interface IBitmapData : IDisposable
    {
        unsafe byte* ScreenData { get; }
        int Stride { get; }
        Rectangle Rectangle { get; }
    }
}
