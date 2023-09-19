using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI listText;

    public string[] items;
    public string[] displayItems;
    [SerializeField] bool[] striked;

    // Start is called before the first frame update
    void Start()
    {
        //test
        items = new string[] {"Food", "Not Food", "Extra Not Food"};
        displayItems = new string[items.Length];
        striked = new bool[items.Length];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    ///List Display toggle
    ///Text Gen from array
    ///strikethough
    void UpdateDisplay()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (striked[i])
            {
                displayItems[i] = "<i>" + items[i] + "</i>";
            }
            else
            {
                displayItems[i] = items[i];
            }
        }
        BuildList();
    }
    //For later implementation
    //talks to inventory to see if it has what it needs.
    void Mark(int i)
    {
        if (striked[i])
        {
            striked[i] = false;
        }
        else
        {
            striked[i] = true;
        }
    }
    void BuildList()
    {
        listText.text = "";
        for( int i = 0; i < displayItems.Length; i++)
        {
            listText.text += displayItems[i] + "\n";
        }
    }
}
