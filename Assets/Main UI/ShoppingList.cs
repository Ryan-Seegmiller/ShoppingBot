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

    [SerializeField] GameManager gm;

    [SerializeField] int[] shopListVals;
    [SerializeField] string[] shopListNames;
    int collected;
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
        if(displayItems.Length < 1 || striked.Length < 1)
        {
            displayItems = new string[GameManager.Instance.inventorySize];
            striked = new bool[GameManager.Instance.inventorySize];
        }
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
            collected = GameManager.Instance.shoppingList[i] - GameManager.Instance.inventory[i];
            itemID current = (itemID)GameManager.Instance.shoppingList[i];
            //string currentName = current.ToString();
            if(collected <= 0)
            {
                striked[i] = true;
            }
            if (striked[i])
            {
                //TODO mess with tags and effects to change striked and not striked
                //there is a color tag (look it up)

                //ToDo replace shopListNames with Item Names via GameManager
                displayItems[i] = "<s><i>" + GameManager.Instance.ItemName(current) + "</i></s>";
            }
            else
            {
                //replace 4 with get from game manager "i" in inventory
                
                displayItems[i] = "<b>" + collected +" "+ GameManager.Instance.ItemName(current) + "</b>";
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
