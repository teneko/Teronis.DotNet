

namespace Teronis.Drawing
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
