using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerCounter;

    public float totalTime = 0;
    public static int minutes;
    public static float seconds;
    public static string time;
    private static bool isStopped = false;

    // Update is called once per frame
    void Update()
    {
        if(!isStopped)
        {
            float t = totalTime + Time.time;
            minutes = ((int)t / 60);
            seconds = (t % 60);
            timerCounter.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");
            time = minutes.ToString("00") + ":" + seconds.ToString("00.00");
        }
        else
        {
            this.enabled = false;
        }
    }

    public static void StopTimer()
    {
        isStopped = true;
    }
}
