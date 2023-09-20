using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalc : MonoBehaviour
{
    public static ScoreCalc instance;

    public int scoreVal;

    void Start()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetScore()
    {
        return scoreVal;
    }
}
