using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    
    public Text finishTime;

    // Start is called before the first frame update
    void Start()
    {
        finishTime.text = "Your time: " + Timer.time;
    }
}
