using UnityEngine.UI;

internal class CCInputField : InputField
{
    protected override void OnDisable()
    {
        text = "";
        base.OnDisable();
    }
}