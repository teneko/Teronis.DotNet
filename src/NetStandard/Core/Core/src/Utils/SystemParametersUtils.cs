

namespace Teronis.Utils
{
    public static class SystemParametersUtils
    {
        public static int CalculateKeyboardDelayInterval(int keyboardDelay)
            => (keyboardDelay + 1) * 250;
    }
}
