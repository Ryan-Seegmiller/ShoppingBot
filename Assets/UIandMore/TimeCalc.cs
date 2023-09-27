using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCalc : MonoBehaviour
{
    public static TimeCalc instance;

    public int timer;
    public bool ticking;

    string timeString = "";
    int timeholder;
    int timeholder2;
    int timeholder3;
    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartTimer();
    }

    public string GetTimeString()
    {
        //min
        timeString = "";
        timeholder = (timer / 60) / 100;
        if (timeholder >= 1)
        {
            if(timeholder < 10)
            {
                timeString += "0";
            }
            timeString += timeholder + ":";
        }
        else { timeString += "00:"; }
        //sec
        timeholder2 = timer / 100;
        if (timeholder2 >= 1)
        {
            if (timeholder2 < 10)
            {
                timeString += "0";
            }
            timeString += timeholder2 + ":";
        }
        else { timeString += "00:"; }
        //ms
        timeholder3 = timer - (timeholder * 6000) - (timeholder2 * 100);
        if(timeholder3 < 10) { timeString += "0"; }
        timeString += timeholder3;

        return timeString;
    }

    public void ResetTimer()
    {
        timer = 0;
    }
    public void SetTimer(int i)
    {
        timer = i;
    }
    public void StartTimer()
    {
        ticking = true;
        StartCoroutine(Tick());
    }
    public void PauseTimer()
    {
        ticking = false;
    }

    IEnumerator Tick()
    {
        while (ticking)
        {
            yield return new WaitForSeconds(0.01f);
            timer++;
        }
    }
}
