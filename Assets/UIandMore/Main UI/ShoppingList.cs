using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using gamemanager;

public class ShoppingList : MonoBehaviour
{
    public static ShoppingList instance;

    [SerializeField] GameObject closedList;
    [SerializeField] GameObject fullList;
    [SerializeField] TextMeshProUGUI listText;
    bool showingList = false;

    //[SerializeField] GameManager gm;

    bool collected;
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
        for(int i = 0; i < displayItems.Length; i++)
        {
            int ignorer = 0;
            collected = false;
            //collected = GameManager.Instance.shoppingList[i] == GameManager.Instance.inventory[i];
            for (int j = 0; j < i; j++)
            {
                ignorer = 0;
                if (!striked[i])
                {
                    //if they match, ignore that many in.
                    if (GameManager.Instance.inventory[i] == GameManager.Instance.inventory[j])
                    {
                            ignorer++;
                    }
                }
            }
            for (int j = 0; j < GameManager.Instance.inventorySize; j++)
            {
                if(GameManager.Instance.inventory[i] == GameManager.Instance.shoppingList[j])
                {
                    if (ignorer > 0)
                    {
                        ignorer--;
                    }
                    else
                    {
                        collected = true;
                    }
                }
            }
            //^^^Change out for if it's anywhere in the list and prevent double checking on same item
            string currentName = GameManager.Instance.ItemName(GameManager.Instance.shoppingList[i]);
            if(collected)
            {
                striked[i] = true;
            }
            else
            {
                striked[i] = false;
            }
            if (striked[i])
            {
                //TODO mess with tags and effects to change striked and not striked
                //there is a color tag (look it up)

                //ToDo replace shopListNames with Item Names via GameManager
                displayItems[i] = "<s><i>" + currentName + "</i></s>";
            }
            else
            {
                //replace 4 with get from game manager "i" in inventory
                
                displayItems[i] = "<b>"+ currentName + "</b>";
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
        listText.text = "Gather\n";
        for( int i = 0; i < displayItems.Length; i++)
        {
            listText.text += displayItems[i] + "\n";
        }
    }
    #endregion
}
