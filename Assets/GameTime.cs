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
        // finishTime.text = "Hello";
        finishTime.text = "Your time: " + Timer.finalTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
