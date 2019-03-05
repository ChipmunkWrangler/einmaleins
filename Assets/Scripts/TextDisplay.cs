using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

internal class TextDisplay : MonoBehaviour
{
    private Text textField;

    protected void SetText(string text)
    {
        GetTextField().text = text;
    }

    protected string GetText()
    {
        return GetTextField().text;
    }

    protected Text GetTextField()
    {
        if (textField == null)
        {
            textField = gameObject.GetComponent<Text>();
            Assert.IsNotNull(textField);
        }

        return textField;
    }

    private void Start()
    {
        GetTextField().text = "";
    }
}