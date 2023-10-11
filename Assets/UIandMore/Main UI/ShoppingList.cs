using Items;
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
            displayItems = new string[ItemManager.instance.inventorySize];
            striked = new bool[ItemManager.instance.inventorySize];
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
        
        for(int i = 0; i < displayItems.Length; i++)
        {
            //Until further fixing, items will have to be collected in exact order
            collected = ItemManager.instance.shoppingList[i] == ItemManager.instance.inventory[i];
            string currentName = ItemManager.instance.ItemName(ItemManager.instance.shoppingList[i]);
            /*
            int ignorer = 0;
            collected = false;

            //Until further fixing, items will have to be collected in exact order
            //collected = ItemManager.instance.shoppingList[i] == ItemManager.instance.inventory[i];

            if (ItemManager.instance.ItemTotalCount(ItemManager.instance.inventory[i], ItemManager.instance.shoppingList) > 1)
            {
                ignorer = 0;
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (ItemManager.instance.inventory[j] == ItemManager.instance.shoppingList[i])
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
                    for(int j = 0; j < ItemManager.instance.inventorySize; j++)
                    {
                        if (ItemManager.instance.ListHasItem(ItemManager.instance.inventory[i]))
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
            else if(ItemManager.instance.ListHasItem(ItemManager.instance.inventory[i]))
            {
                striked[i] = true;
            }

            */

            if (collected)
            {
                striked[i] = true;
            }
            else
            {
                striked[i] = false;
            }
            if (ItemManager.instance.completionList[i])
            {
                //TODO mess with tags and effects to change striked and not striked
                //there is a color tag (look it up)

                //ToDo replace shopListNames with Item Names via ItemManager
                displayItems[i] = "<s><i>" + currentName + "</i></s>";
            }
            else
            {
                //replace 4 with get from game manager "i" in inventory
                
                displayItems[i] = "<b>"+ currentName + "</b>";

                if (ItemManager.instance.inventory[i] != -1)
                {
                    displayItems[i] += " <b>X<b>";
                }
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
        /*
        for(int i=0; i < ItemManager.instance.shoppingList.Length; i++)
        {
            if (!ItemManager.instance.inventory.Contains(ItemManager.instance.shoppingList[i]))
            {
                listText.text += ItemManager.instance.ItemName(ItemManager.instance.shoppingList[i]) + "\n";
            }
            else
            {
                currIng = 0;
                theItem = ItemManager.instance.shoppingList[i];
                int difference = ItemManager.instance.ItemTotalCount(theItem, ItemManager.instance.shoppingList) - ItemManager.instance.ItemTotalCount(theItem, ItemManager.instance.inventory);
                //if more of item in list than in inv
                if (difference > 0)
                {
                    currIng = 0;
                    int theItem = ItemManager.instance.shoppingList[i];
                    int difference = ItemManager.instance.ItemTotalCount(theItem, ItemManager.instance.shoppingList) - ItemManager.instance.ItemTotalCount(theItem, ItemManager.instance.inventory);
                    //if more of item in list than in inv
                    if (difference > 0)
                    {
                        ignores[theItem] = difference;
                    }
                    
                }
                
            }
        }*/
    }
    #endregion

}
