using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenshot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            ScreenCapture.CaptureScreenshot("SomeLevel", 4);
        }
    }
}
