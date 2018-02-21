using System.Linq;

public class MDCulture {
    static System.Globalization.CultureInfo CI;

	public static System.Globalization.CultureInfo GetCulture()
	{
		if (CI == null) {
			string langId = PreciseLocale.GetLanguageID().Replace('_', '-');
			CI = System.Globalization.CultureInfo.GetCultures (System.Globalization.CultureTypes.AllCultures).FirstOrDefault (x => x.Name == langId);
		}
		return CI;
	}

}
