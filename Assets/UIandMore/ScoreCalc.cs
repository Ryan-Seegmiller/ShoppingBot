using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalc : MonoBehaviour
{
    public static ScoreCalc instance;

    public int scoreVal;

    //Time intervals
    int[] testValues1 = new int[] {30000, 60000, 90000, 120000, 180000};
    //currently 5, 10, 15, 20, 30 minutes

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
        int t = TimeCalc.instance.timer; //TODO replace with Game Manager calling it
        int j = testValues1.Length;
        for(int i = 0; i < testValues1.Length; i++)
        {
            if(t < testValues1[i])
            {
                //less time spent = more score per threshold
                scoreVal += 100 * j / 2;
            }
            j--;
        }

        //Inventory Accurracy
        for(int i = 0; i < ItemManager.instance.inventorySize; i++)
        {
            //check how many items were correct, +100 for each?
            if (ItemManager.instance.completionList[i])
            {
                //bigger numer = more incentive
                scoreVal += 500;
            }
        }
    }
}
