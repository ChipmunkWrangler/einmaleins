using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadXMLData : MonoBehaviour
{
	[SerializeField] Text inputField;
	[SerializeField] UnityEngine.UI.Text statusLine = null;

	public void LoadFromInputField ()
	{
		try {
			string s = inputField.text;
			XMLSerializationHandler.LoadFromString (s);
		} catch (System.Exception ex) {
			statusLine.text = AssemblyCSharp.ExceptionPrettyPrint.Msg (ex);
		}	
	}
}
