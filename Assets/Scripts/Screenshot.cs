using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    private static int i = -1;

    private static void MakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot($"screenshot{++i}.png");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            MakeScreenshot();
        }
    }
}
