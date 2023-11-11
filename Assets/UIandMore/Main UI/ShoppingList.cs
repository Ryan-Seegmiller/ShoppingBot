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
            collected = ItemManager.instance.shoppingList[i] == ItemManager.instance.inventory[i];
            string currentName = ItemManager.instance.ItemName(ItemManager.instance.shoppingList[i]);

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
                //there is a color tag (look it up)
                displayItems[i] = "<s><i>" + currentName + "</i></s>";
            }
            else
            {
                displayItems[i] = "<b>"+ currentName + "</b>";
            }
        }
        BuildList();
    }
    //Note: list will not render properly if the shoplist is randomized while correct items are in inventory.
    //should never occur but is a bug
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
