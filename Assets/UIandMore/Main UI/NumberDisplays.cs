using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberDisplays : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyAmt;
    [SerializeField] TextMeshProUGUI timer;

    private void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        moneyAmt.text = "$" + GameManager.instance.cash.ToString();
        timer.text = TimeCalc.instance.GetTimeString();
    }
}
