// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Windows.Drawing
{
    public class Pixel : ITwoDimensionalPattern
    {
        public RGBColor Color;
        public Position Position;

        bool ITwoDimensionalPattern.ColorSupport => true;

        public Pixel(RGBColor color, Position position)
        {
            Color = color;
            Position = position;
        }

        public static implicit operator RGBColor(Pixel clrPos)
        {
            return clrPos.Color;
        }

        public static implicit operator Position(Pixel clrPos)
        {
            return clrPos.Position;
        }

        void ITwoDimensionalPattern.GetPosition(out Position position) => position = Position;

        void ITwoDimensionalPattern.GetColor(out RGBColor color) => color = Color;
    }
}
