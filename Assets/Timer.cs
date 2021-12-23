using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerCounter;

    private static float totalTime = 0;
    private static bool isStopped = false;
    public static int minutes;
    public static float seconds;
    public static string time;

    // Update is called once per frame
    void Update()
    {
        if(!isStopped)
        {
            totalTime += Time.deltaTime;
            minutes = ((int)totalTime / 60);
            seconds = (totalTime % 60);
            timerCounter.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");
            time = minutes.ToString("00") + ":" + seconds.ToString("00.00");
        }
        else
        {
            //Stop Counting
        }
    }

    public static void StopTimer()
    {
        isStopped = true;
    }

    public static void ResetTimer()
    {
        isStopped = false;
        totalTime = 0;
    }
}
