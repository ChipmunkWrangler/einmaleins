using UnityEngine;
using System;

namespace AssemblyCSharp
{
	public static class ExceptionPrettyPrint
	{
		public static string Msg (Exception ex)
		{
			Debug.Log ("Exception " + ex);
			string s = ex.Message;
			while (ex.InnerException != null) {
				ex = ex.InnerException;
				s += " " + ex.Message;
			}
			return s;
		}
	}
}

