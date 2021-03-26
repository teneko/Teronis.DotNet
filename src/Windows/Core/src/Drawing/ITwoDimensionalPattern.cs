// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Windows.Drawing
{
    public interface ITwoDimensionalPattern
    {
        bool ColorSupport { get; }

        void GetPosition(out Position position);
        void GetColor(out RGBColor color);
    }
}
