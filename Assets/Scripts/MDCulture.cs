using System.Globalization;
using System.Linq;

internal class MDCulture
{
    private static CultureInfo ci;

    public static CultureInfo GetCulture()
    {
        if (ci == null)
        {
            var langId = PreciseLocale.GetLanguageID().Replace('_', '-');
            ci = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.Name == langId);
        }

        return ci;
    }
}