using System.Globalization;

namespace Teronis.Utils
{
    public static class CultureInfoUtils
    {
        public static bool DoesCultureNameExists(string name)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (var culture in cultures) {
                if (culture.Name == name) {
                    return true;
                }
            }

            return false;
        }
    }
}
