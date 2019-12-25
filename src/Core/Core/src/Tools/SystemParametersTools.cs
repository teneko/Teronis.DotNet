

namespace Teronis.Tools
{
    public static class SystemParametersTools
    {
        public static int CalculateKeyboardDelayInterval(int keyboardDelay)
            => (keyboardDelay + 1) * 250;
    }
}
