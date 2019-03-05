using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

internal class LoadXMLData : MonoBehaviour
{
    [SerializeField] private Text inputField;
    [SerializeField] private Text statusLine;

    public void LoadFromInputField()
    {
        try
        {
            XMLSerializationHandler.LoadFromString(inputField.text);
        }
        catch (Exception ex)
        {
            statusLine.text = ExceptionPrettyPrint.Msg(ex);
        }
    }

    public void LoadFromFile()
    {
        try
        {
            XMLSerializationHandler.LoadFromFile();
            statusLine.text = LocalizationManager.GetTermTranslation("Data loaded.");
        }
        catch (Exception ex)
        {
            statusLine.text = ExceptionPrettyPrint.Msg(ex);
        }
    }
}