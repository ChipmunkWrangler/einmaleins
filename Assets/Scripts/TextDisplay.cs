using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    Text textField;

    protected void SetText(string text)
    {
        GetTextField().text = text;
    }

    protected string GetText() => GetTextField().text;

    protected Text GetTextField()
    {
        if (textField == null)
        {
            textField = gameObject.GetComponent<Text>();
            UnityEngine.Assertions.Assert.IsNotNull(textField);
        }
        return textField;
    }

    void Start()
    {
        GetTextField().text = "";
    }
}
