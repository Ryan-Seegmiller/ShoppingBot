using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamemanager;

public class ScoreCalc : MonoBehaviour
{
    public static ScoreCalc instance;

    public int scoreVal;

    int[] testValues1 = new int[] {6000, 12000, 24000};

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
        scoreVal = 0;
    }

    public int GetScore()
    {
        CalcScore();
        return scoreVal;
    }
    public void CalcScore()
    {
        scoreVal = 0;

        //Time
        int t = TimeCalc.instance.timer;
        int j = testValues1.Length;
        for(int i = 0; i < testValues1.Length; i++)
        {
            if(t < testValues1[i])
            {
                scoreVal += 10 * j / 2;
            }
            j--;
        }

        //Inventory Accurracy
        for(int i = 0; i < GameManager.Instance.inventorySize; i++)
        {
            //check hom many items were correct, +30 for each?
        }
    }
}
