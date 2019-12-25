

namespace Teronis.Tools
{
    public static class MathTools
    {
        public static int ModZeroBasedNumber(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static int ModOneBasedNumber(int x, int m)
        {
            int r = x % m;
            return r <= 0 ? r + m : r;
        }
    }
}
