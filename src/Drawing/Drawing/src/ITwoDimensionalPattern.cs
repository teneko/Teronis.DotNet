

namespace Teronis.Drawing
{
    public interface ITwoDimensionalPattern
    {
        bool ColorSupport { get; }

        void GetPosition(out Position position);
        void GetColor(out RGBColor color);
    }
}
