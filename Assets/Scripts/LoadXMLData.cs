using UnityEngine;
using UnityEngine.UI;

class LoadXMLData : MonoBehaviour
{
    [SerializeField] Text inputField = null;
    [SerializeField] Text statusLine = null;

    public void LoadFromInputField()
    {
        try
        {
            XMLSerializationHandler.LoadFromString(inputField.text);
        }
        catch (System.Exception ex)
        {
            statusLine.text = ExceptionPrettyPrint.Msg(ex);
        }
    }

    public void LoadFromFile()
    {
        try
        {
            XMLSerializationHandler.LoadFromFile();
            statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation("Data loaded.");
        }
        catch (System.Exception ex)
        {
            statusLine.text = ExceptionPrettyPrint.Msg(ex);
        }
    }
}
