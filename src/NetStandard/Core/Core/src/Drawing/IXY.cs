// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Drawing
{
    public interface IXY : IEquatable<IXY>
    {
        int X { get; }
        int Y { get; }
    }
}
