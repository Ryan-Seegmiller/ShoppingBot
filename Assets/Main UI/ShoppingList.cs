using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    public static ShoppingList instance;

    [SerializeField] GameObject closedList;
    [SerializeField] GameObject fullList;
    [SerializeField] TextMeshProUGUI listText;
    bool showingList = false;

    //[SerializeField] GameObject gm;

    [SerializeField] int[] shopListVals;
    [SerializeField] string[] shopListNames;
    [SerializeField] int[] collected;
    public string[] displayItems;
    [SerializeField] bool[] striked;

    [SerializeField] GameObject pMenu;
    [SerializeField] GameObject mScreen;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton Pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //test
        //would be set to game manager's preset
        shopListNames = new string[] {"Food", "Not Food", "Extra Not Food"};
        displayItems = new string[shopListNames.Length];
        striked = new bool[shopListNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        KeyCheck();
    }

    //List Display toggle
    //Text Gen from array
    //strikethough

    /*
    #region PassableVals
    public void SetShopList(int[] ia, string[] sa)
    {
        shopListVals = ia;
        shopListNames = sa;
        striked = new bool[ia.Length];
        collected = new int[ia.Length];
    }
    public void SetInv(int[] ia)
    {
        if(ia.Length == collected.Length)
        {
            for(int i = 0; i < ia.Length; i++)
            {
                collected[i] = ia[i];
            }
        }
    }
    #endregion
    */
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
        for(int i = 0; i < shopListNames.Length; i++)
        {
            if (striked[i])
            {
                displayItems[i] = "<i>" + shopListNames[i] + "</i>";
            }
            else
            {
                //replace 4 with get from game manager "i" in inventory
                displayItems[i] = "<b>" + 4 +" "+ shopListNames[i] + "</b>";
            }
        }
        BuildList();
    }
    //For later implementation
    //talks to inventory to see if it has what it needs.
    public void MarkItem(int i)
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
