using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerCounter;

    private float startTime;

    public static string finalTime;
    public static int finalMinutes;
    public static float finalSeconds;
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;
        string minutes = ((int) t/60).ToString("00");
        string seconds = (t % 60).ToString("00.00");

        if (BossEnemy.bossHealth != 0)
        {
            timerCounter.text = minutes + ":" + seconds;
        }
        else
        {
            finalTime = minutes + ":" + seconds;
            finalMinutes = int.Parse(minutes);
            finalSeconds = float.Parse(seconds);
            return;
        }
    }
}
