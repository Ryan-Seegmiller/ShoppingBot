using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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
            //Just build list when list is opened not every frame that it's showing
            //UpdateDisplay();
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
        if (showingList)
        {
            UpdateDisplay();
        }
    }
    #region TasksOpen
    void UpdateDisplay()
    {
        /*
        for(int i = 0; i < displayItems.Length; i++)
        {
            string currentName = GameManager.Instance.ItemName(GameManager.Instance.shoppingList[i]);

            int ignorer = 0;
            collected = false;

            //Until further fixing, items will have to be collected in exact order
            //collected = GameManager.Instance.shoppingList[i] == GameManager.Instance.inventory[i];

            if (GameManager.Instance.ItemTotalCount(GameManager.Instance.inventory[i], GameManager.Instance.shoppingList) > 1)
            {
                ignorer = 0;
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (GameManager.Instance.inventory[j] == GameManager.Instance.shoppingList[i])
                        {
                            ignorer++;
                        }
                    }
                }
                if(ignorer <= i)
                {
                    collected = false;
                }
                else
                {
                    for(int j = 0; j < GameManager.Instance.inventorySize; j++)
                    {
                        if (GameManager.Instance.ListHasItem(GameManager.Instance.inventory[i]))
                        {
                            if(ignorer > 0)
                            {
                                ignorer--;
                            }
                            else
                            {
                                striked[j] = true;
                            }
                        }
                    }
                }
            }
            else if(GameManager.Instance.ListHasItem(GameManager.Instance.inventory[i]))
            {
                striked[i] = true;
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
        }*/
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

        int[] ignores = new int[10];
        int currIng;
        /*
        for( int i = 0; i < displayItems.Length; i++)
        {
            listText.text += displayItems[i] + "\n";
        }*/
        for(int i=0; i < GameManager.Instance.shoppingList.Length; i++)
        {
            if (!GameManager.Instance.inventory.Contains(GameManager.Instance.shoppingList[i]))
            {
                listText.text += GameManager.Instance.ItemName(GameManager.Instance.shoppingList[i]) + "\n";
            }
            else
            {
                if(i > 0)
                {
                    currIng = 0;
                    int theItem = GameManager.Instance.shoppingList[i];
                    int difference = GameManager.Instance.ItemTotalCount(theItem, GameManager.Instance.shoppingList) - GameManager.Instance.ItemTotalCount(theItem, GameManager.Instance.inventory);
                    //if more of item in list than in inv
                    if (difference > 0)
                    {
                        ignores[theItem] = difference;
                    }
                    
                }
            }
        }
    }
    #endregion

}
