using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberDisplays : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyAmt;

    public int money;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateDisplay()
    {
        moneyAmt.text = money.ToString();
    }
}
