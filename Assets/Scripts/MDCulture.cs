using System.Linq;

public class MDCulture {
	static System.Globalization.CultureInfo ci;

	public static System.Globalization.CultureInfo GetCulture()
	{
		if (ci == null) {
			string langId = PreciseLocale.GetLanguageID().Replace('_', '-');
			ci = System.Globalization.CultureInfo.GetCultures (System.Globalization.CultureTypes.AllCultures).FirstOrDefault (x => x.Name == langId);
		}
		return ci;
	}

}
