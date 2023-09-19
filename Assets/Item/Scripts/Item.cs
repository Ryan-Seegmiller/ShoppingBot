using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //HOW TO ADD ITEMS
    //Step 1: Add item in the 'itemID' enum. (Make sure it's positioned with other items in the same category)
    //Step 2: Add the name of the item in the 'itemName' array.
    //Step 3: Add the cost of the item in the 'itemCost' array and comment the item name beside it.
    //Step 4: Add item model as a disabled child of the item gameObject, and then place the model into the itemModel array in the same position as the itemID.
    //NOTE: Step 2-4 must be organized in the same order as the 'itemID' array. Example: if 'strawberry' is 3rd in itemID (index 2), the strawberry's itemName, itemCost and itemModel must also be in the 3rd position (index 2)
    //Step 5: Adjust the int of the same category in the 'categorySize' array to match the number of items in that category

    //HOW TO ADD ITEM CATEGORIES
    //Step 1: Add a comment with the category name at the end of the 'itemID' array, and place all items belonging to this category after that comment
    //Step 2: Add the category name into the 'itemCategory' enum
    //Step 3: Add a new int to the 'categorySize' array and comment which category that int belongs to. Int value should match the number of individual items in that category.
    //NOTE: The category must be in the same position for all enums/arrays. If your new category is 4th in the 'itemID' enum, it should also be 4th in the'itemCategory' enum and 'categorySize' array 



    //ITEMS
    //Item id enum
    enum itemID {
        //Fruit
        apple,
        banana,
        strawberry,
        pineapple,
        //Cheese
        provolone, //The best cheese
        cheddar,
        pepperjack,
        //Tool
        screwdriver,
        hammer,
        wrench
    };
    //Item name array
    string[] itemName = {
        //Fruit
        "Apple",
        "Banana",
        "Strawberry",
        "Pineapple",
        //Cheese
        "Provolone",
        "Cheddar",
        "Pepperjack",
        //Tool
        "Screwdriver",
        "Hammer",
        "Wrench",
    };
    //Item cost array
    int[] itemCost = {
        //Fruit
        3, //Apple
        4, //Banana
        1, //Strawberry
        5, //Pineapple
        //Cheese
        12, //Provolone
        8, //Cheddar
        10, //Pepperjack
        //Tool
        16, //Screwdriver
        25, //Hammer
        22, //Wrench
    };

    //ITEM CATEGORIES
    //Item category enum
    enum itemCategory {
        fruit,
        cheese,
        tool
    };
    //Category sizes. Category position in this array is the same as the itemCategory enum. (i.e. fruit = 0, cheese = 1, tool = 2, etc.)
    int[] categorySize = {
        4, //Fruit
        3, //Cheese
        3, //Tool
    };

    //ITEM VALUES
    public int id = -1; //Item's ID. -1 = null/no item assigned
    public int cost = 0; //The cash cost of this item

    //References to the item models (children of the item gameObject). Change/add to these in the inspector, not here.
    [SerializeField] GameObject[] itemModels;



    void Start()
    {
        ChooseFromCategory((int)itemCategory.fruit);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            ChooseFromCategory((int)itemCategory.fruit);
        }
    }

    
    //Assigns item value to a random item from a specified category
    void ChooseFromCategory(int categoryID) //Use the 'itemCategory' enum to choose the category to pick from. Example: (int)itemCategory.fruit
    {
        int itemIdMin = categoryID > 0 ? categorySize[categoryID - 1] : 0; //The item ID of the first available item in the chosen category
        int itemIdMax = itemIdMin + categorySize[categoryID] - 1; //The item ID of the last available item in the chosen category
        int newChoice = Mathf.RoundToInt(Random.Range((float)itemIdMin - 0.5f, (float)itemIdMax + 0.4f)); //Choose which item ID to assign the item
        //Assign item to new item choice
        AssignItem(newChoice);
    }


    //Assigns item values and model based on itemID
    void AssignItem(int newID)
    {
        print(newID);
        //Assign the item ID
        id = newID;
        //Assign the item model
        DisableAllModels();
        itemModels[id].SetActive(true);
        //Assign cost
        cost = itemCost[id];
        
    }
    //Disable all item models
    void DisableAllModels()
    {
        for (int i = 0; i < itemModels.Length; i++)
        {
            print(itemModels[i].name);
            itemModels[i].SetActive(false);
        }
    }

}
