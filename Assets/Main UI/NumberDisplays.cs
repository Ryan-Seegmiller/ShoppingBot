using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberDisplays : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyAmt;
    [SerializeField] TextMeshProUGUI timer;

    //Replaceable with call to player or game manager's values
    public int money;
    

    public void UpdateDisplay()
    {
        moneyAmt.text = money.ToString();

    }
}
