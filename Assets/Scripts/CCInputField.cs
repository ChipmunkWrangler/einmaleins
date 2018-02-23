using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CCInputField : UnityEngine.UI.InputField
{
    protected override void OnDisable()
    {
        text = "";
        base.OnDisable();
    }
}
