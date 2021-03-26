// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Drawing
{
    public interface IXYZ : IXY, IEquatable<IXYZ>
    {
        int Z { get; }
    }
}
