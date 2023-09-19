using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemValue : MonoBehaviour
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
    public enum itemID {
        //Food
        apple,
        banana,
        strawberry,
        pineapple,
        //Clothes
        shoe,
        hat,
        glasses,
        //Tools
        screwdriver,
        hammer,
        wrench
    };
    //Item name array
    string[] itemName = {
        //Food
        "Apple",
        "Banana",
        "Strawberry",
        "Pineapple",
        //Clothes
        "Shoe",
        "Hat",
        "Glasses",
        //Tools
        "Screwdriver",
        "Hammer",
        "Wrench",
    };
    //Item cost array
    int[] itemCost = {
        //Food
        3, //Apple
        4, //Banana
        1, //Strawberry
        5, //Pineapple
        //Clothes
        12, //Shoe
        8, //Hat
        10, //Glasses
        //Tools
        16, //Screwdriver
        25, //Hammer
        22, //Wrench
    };

    //ITEM CATEGORIES
    //Item category enum
    public enum itemCategory {
        food,
        clothes,
        tools
    };
    //Category sizes. Category position in this array is the same as the itemCategory enum. (i.e. fruit = 0, cheese = 1, tool = 2, etc.)
    int[] categorySize = {
        4, //Food
        3, //Clothes
        3, //Tools
    };

    //ITEM VALUES
    public int id = -1; //Item's ID. -1 = null/no item assigned
    [HideInInspector] public int Cost = 0; //The cash cost of this item
    [HideInInspector] public string Name = ""; //The name of this item

    //References to the item models (children of the item gameObject). Change/add to these in the inspector, not here.
    [SerializeField] GameObject[] itemModels;



    void Start()
    {
        EnableModel(); //Disable all models at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            RandomiseFromCategory(itemCategory.food);
            //RandomiseItem();
        }
    }

    
    //Assigns item value to a random item from a specified category
    void RandomiseFromCategory(itemCategory categoryID) //Use the 'itemCategory' enum to choose the category to pick from. Example: (int)itemCategory.fruit
    {
        int indexCount = 0; //Get the number of items in the itemID enum before the chosen category starts
        for (int i = 0; i < categorySize.Length - (categorySize.Length - (int)categoryID); i++) { //Repeat for each category before the chosen category
            indexCount += categorySize[i];
        }
        int itemIdMin = (int)categoryID > 0 ? indexCount : 0; //The item ID of the first available item in the chosen category
        int itemIdMax = indexCount + categorySize[(int)categoryID] - 1; //The item ID of the last available item in the chosen category
        int newChoice = Mathf.RoundToInt(Random.Range((float)itemIdMin - 0.5f, (float)itemIdMax + 0.4f)); //Choose which item ID to assign the item
        //Assign item to new item choice
        AssignItem(newChoice);
    }
    //Assigns item value to a random item
    void RandomiseItem()
    {
        int itemIdMin = 0; //The item ID of the first available item in the chosen category
        int itemIdMax = itemCost.Length - 1; //The item ID of the last available item in the chosen category
        int newChoice = Mathf.RoundToInt(Random.Range((float)itemIdMin - 0.5f, (float)itemIdMax + 0.4f)); //Choose which item ID to assign the item
        //Assign item to new item choice
        AssignItem(newChoice);
    }


    //Assigns item values and model based on itemID
    void AssignItem(int newID)
    {
        print(newID); //DEBUG
        //Assign the item ID
        id = newID;
        //Assign the item model
        EnableModel();
        //Assign cost
        Cost = itemCost[id];
        //Assign name
        Name = itemName[id];
        
    }
    //Disable all item models if they're not the correct model for this item's id, and enable the correct one
    void EnableModel()
    {
        for (int i = 0; i < itemModels.Length; i++) //Repeat for every model
        {
            if (i != id) //Check current ID
            {
                itemModels[i].SetActive(false); //Disable model
            } else {
                itemModels[i].SetActive(true);
            }
        }
    }

}
