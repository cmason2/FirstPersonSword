using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public Text theLeaderboard;

    private (int,float,string) first;
    private (int,float,string) second;
    private (int,float,string) third;
    private (int,float,string) fourth;
    private (int,float,string) fifth;

    private (int,float,string) newTime;

    public List<(int,float,string)> timesList;

    // Start is called before the first frame update
    void Start()
    {
        timesList[0] = first;
        timesList[1] = second;
        timesList[2] = third;
        timesList[3] = fourth;
        timesList[4] = fifth;

        newTime = (Timer.finalMinutes, Timer.finalSeconds, Timer.finalTime);

        for (int i = 0; i < 5; i++)
        {
            (int a, float b, _) = timesList[i];
            (int c, float d, _) = newTime;

            if (c < a)
            {
                updateList(i);
            }
            else if (c == a && d <= b)
            {
                updateList(i);
            }
        }
    }

    public void updateList(int x)
    {
        for (int i = 4; i > x; i--)
        {
            timesList[i] = timesList[i-1];
        }

        timesList[x] = newTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
