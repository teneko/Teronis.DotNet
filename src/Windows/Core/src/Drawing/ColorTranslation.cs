// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Windows.Drawing
{
    public class ColorTranslation
    {
        public RGBColor FromColor;
        public RGBColor ToColor;

        public ColorTranslation() { }

        public ColorTranslation(RGBColor fromColor, RGBColor toColor)
        {
            FromColor = fromColor;
            ToColor = toColor;
        }
    }
}
