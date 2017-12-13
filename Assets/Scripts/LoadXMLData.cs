using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadXMLData : MonoBehaviour {
	[SerializeField] Text inputField;
	[SerializeField] UnityEngine.UI.Text statusLine = null;

	public void LoadFromInputField () {
		try {
			XMLSerializationHandler.LoadFromString( inputField.text );
		} catch (System.Exception ex) {
			statusLine.text = AssemblyCSharp.ExceptionPrettyPrint.Msg( ex );
		}	
	}

	public void LoadFromFile () {
		try {
			XMLSerializationHandler.LoadFromFile();
			statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation( "Data loaded." );
		} catch (System.Exception ex) {
			statusLine.text = AssemblyCSharp.ExceptionPrettyPrint.Msg( ex );
		}	
	}
}
