using UnityEngine;
using UnityEngine.UI;

public class LoadXMLData : MonoBehaviour {
    [SerializeField] Text InputField = null;
    [SerializeField] Text StatusLine = null;

	public void LoadFromInputField () {
		try {
			XMLSerializationHandler.LoadFromString( InputField.text );
		} catch (System.Exception ex) {
			StatusLine.text = AssemblyCSharp.ExceptionPrettyPrint.Msg( ex );
		}	
	}

	public void LoadFromFile () {
		try {
			XMLSerializationHandler.LoadFromFile();
			StatusLine.text = I2.Loc.LocalizationManager.GetTermTranslation( "Data loaded." );
		} catch (System.Exception ex) {
			StatusLine.text = AssemblyCSharp.ExceptionPrettyPrint.Msg( ex );
		}	
	}
}
