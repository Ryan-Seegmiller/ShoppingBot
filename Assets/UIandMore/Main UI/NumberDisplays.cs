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
        moneyAmt.text = "$" + GameManager.Instance.cash.ToString();
        timer.text = TimeAndScore.instance.GetTimeString();
    }
}
