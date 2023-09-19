using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    [SerializeField] GameObject closedList;
    [SerializeField] GameObject fullList;
    [SerializeField] TextMeshProUGUI listText;
    bool showingList = false;

    public string[] items;
    public string[] displayItems;
    [SerializeField] bool[] striked;

    [SerializeField] GameObject pMenu;
    [SerializeField] GameObject mScreen;

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
        KeyCheck();
    }

    //List Display toggle
    //Text Gen from array
    //strikethough

    void KeyCheck()
    {
        if (showingList)
        {
            UpdateDisplay();
            if (Input.anyKeyDown)
            {
                ToggleList();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleList();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pMenu.SetActive(true);
            mScreen.SetActive(false);
        }
    }
    public void ToggleList()
    {
        fullList.SetActive(!showingList);
        closedList.SetActive(showingList);
        showingList = !showingList;
    }
    #region TasksOpen
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
                displayItems[i] = "<b>" + items[i] + "</b>";
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
    #endregion
}
