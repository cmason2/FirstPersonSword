using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class create_Leaderboard : MonoBehaviour
{
    public Text theLeaderboard;

    public (int,float,string) first;
    public (int,float,string) second;
    public (int,float,string) third;
    public (int,float,string) fourth;
    public (int,float,string) fifth;

    public (int,float,string) newTime;

    public List<(int,float,string)> timesList;

   void Start()
   {  
        theLeaderboard.text = "04:12.35\n04:20.56\n04:36.92\n04:58.34\n05:23.10";

        first = (4, 12.35f, "04:12.35");
        second = (4, 20.56f, "04:20.56");
        third = (4, 36.92f, "04:36.92");
        fourth = (4, 58.34f, "04:58.34");
        fifth = (5, 23.10f, "05:23.10");

        timesList = new List<(int,float,string)>();

        timesList.Add(first);
        timesList.Add(second);
        timesList.Add(third);
        timesList.Add(fourth);
        timesList.Add(fifth);

        if (Timer.finalTime != null)
        {
            newTime = (Timer.finalMinutes, Timer.finalSeconds, Timer.finalTime);

            for (int i = 0; i < 5; i++)
            {
                (int a, float b, string x) = timesList[i];
                (int c, float d, _) = newTime;

                if (c < a)
                {
                    updateList(i);
                    break;
                }
                else if (c == a && d <= b)
                {
                    updateList(i);
                    break;
                }
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

        theLeaderboard.text = "";

        for (int i = x; i < 5; i++)
        {

            (_, _, string y) = timesList[i];
        
            theLeaderboard.text = theLeaderboard.text + y + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // initialiseLeaderboard();
    }
}
