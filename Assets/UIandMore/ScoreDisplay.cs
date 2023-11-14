using Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int totScore = 0;
    int disScore;

    [SerializeField] TextMeshProUGUI timeD;
    string timeT;

    [SerializeField] TextMeshProUGUI itemsD;
    string itemsT;

    // Start is called before the first frame update
    void Start()
    {
        //DisplayText();
    }
    private void OnEnable()
    {
        DisplayText();
        print("enabled score");
    }

    public void DisplayText()
    {
        totScore = ScoreCalc.instance.GetScore();
        StartCoroutine(ShowScore());
    }
    void StringTime()
    {
        timeD.text = "Time Taken\n00:00:00";
        //sets up timer for displaying
        timeT = "Time Taken\n" + TimeCalc.instance.GetTimeString();
    }
    void StringItems()
    {

        itemsD.text = "Items Correct\n0/0";
        //sets up items for displaying
        int correct = 0;
        for(int i = 0; i < ItemManager.instance.inventorySize; i++)
        {
            if (ItemManager.instance.completionList[i])
            {
                correct++;
            }
        }

        itemsT = "Items Correct\n" + correct + "/" + ItemManager.instance.inventorySize;
    }

    IEnumerator ShowScore()
    {
        //Text go brr
        while (disScore < totScore)
        {
            if(totScore - disScore < 100)
            {
                yield return new WaitForSeconds(0.01f);
                disScore++;
            }
            else
            {
                yield return new WaitForSeconds(0.0001f);
                disScore += 5;
            }
            
            scoreText.text = disScore.ToString();
        }
        yield return new WaitForSeconds(0.5f);
        StringTime();
        timeD.text = timeT;
        yield return new WaitForSeconds(0.5f);
        StringItems();
        itemsD.text = itemsT;
    }
}
