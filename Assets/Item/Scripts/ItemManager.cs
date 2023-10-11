using UnityEngine;


namespace Items
{
    //HOW TO ADD ITEMS
    //Step 1: Add item in the 'itemID' enum. (Make sure it's positioned with other items in the same category)
    //Step 2: Add the cost of the item in the 'itemCost' array and comment the item name beside it.
    //Step 3: Add item prefab in the GameManager's item factory scriptableObject.
    //NOTE: Steps 2 and 3 must be organized in the same order as the 'itemID' array. Example: if 'strawberry' is 3rd in itemID (index 2), the strawberry's itemName, itemCost and item prefab must also be in the 3rd position (index 2)
    //Step 4: Adjust the int of the same category in the 'categorySize' array to match the number of items in that category.

    //HOW TO ADD ITEM CATEGORIES
    //Step 1: Add a comment of the category name at the end of the 'itemID' array, and place all items belonging to this category after that comment
    //Step 2: Add the category name into the 'itemCategory' enum
    //Step 3: Add a new int to the 'categorySize' array and comment which category that int belongs to. Int value should match the number of individual items in that category.
    //NOTE: The category must be in the same position for all enums/arrays. If your new category is 4th in the 'itemID' enum, it should also be 4th in the'itemCategory' enum and 'categorySize' array 

    //Item id enum
    public enum ItemID
    {
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

    //Item category enum
    public enum ItemCategory
    {
        food,
        clothes,
        tools
    };
    public class ItemManager : MonoBehaviour
    {

        public static ItemManager instance;

        //Item factory
        public ItemFactory itemFactory;


        //ITEMS
        //Item cost array
        public int[] itemCost = {
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
        //Category sizes. Category position in this array is the same as the itemCategory enum. (i.e. fruit = 0, cheese = 1, tool = 2, etc.)
        public int[] categorySize = {
            4, //Food
            3, //Clothes
            3, //Tools
        };

        //PLAYER INVENTORY
        public int cash = 100; //Player cash
        public int inventorySize = 10; //Inventory size
        //Player inventory
        [HideInInspector] public int[] inventory;
        //Shopping list
        [HideInInspector] public int[] shoppingList;
        //Shopping list complete
        [HideInInspector] public bool[] completionList;


        void Awake()
        {
            //Singleton
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            //Assign inventory and shopping list array size, and set each item to -1
            inventory = new int[inventorySize];
            shoppingList = new int[inventorySize];
            completionList = new bool[inventorySize];
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i] = -1; //Assign every item in inventory to -1
                shoppingList[i] = -1; //Assign every item in list to -1
                completionList[i] = false;
            }
        }

        void Update()
        {
            //DEBUG
            if (Input.GetKeyDown(KeyCode.P))
            {
                itemFactory.InstanceItem(ItemCategory.clothes, new Vector3(0, 0, 0));
            }
        }


        //Returns the string name of an item from given ID
        public string ItemName(int itemID)
        {
            //Assign name
            string nameLower = ((ItemID)itemID).ToString();
            return char.ToUpper(nameLower[0]) + nameLower.Substring(1);
        }

        //Counts the number of times 'itemID' item appears in 'intArr' array
        public int ItemTotalCount(int itemID, int[] intArr)
        {
            int result = 0;
            for (int i = 0; i < intArr.Length; i++)
            {
                if (intArr[i] == itemID)
                {
                    result++;
                }
            }
            return result;
        }





        //INVENTORY

        //Adds an item to the first empty item slot (-1) in the inventory array
        public void AddItem(int itemID)
        {
            //Add item to inventory
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == -1) //Check if index is available
                {
                    inventory[i] = itemID; //Add item to the available index
                    break;
                }
            }

            //Update shopping list completion
            UpdateCompletion();

            //DEBUG
            /*
            for (int d = 0; d < inventory.Length; d++)
            {
                print(d + ":  " + inventory[d]);
            }
            */
        }

        //Checks if there's any more space in your inventory
        public bool CheckInventorySpace()
        {
            foreach (int i in inventory) //Check each item in inventory array
            {
                if (i == -1) //If a -1 is found, the inventory has space
                {
                    return true;
                }
            }
            return false; //No empty space (-1) was found, return false
        }

        public void RemoveRandomItem()
        {

        }



        //SHOPPING LIST

        //Returns true if the shopping list has the given item, whether it's completed or not
        public bool ListHasItem(int itemID)
        {
            if (shoppingList.Length > 0)
            {
                for (int i = 0; i < shoppingList.Length; i++)
                {
                    if (shoppingList[i] == itemID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Returns true if the shopping list has the given item AND that item isn't in your inventory
        public bool ListNeedsItem(int itemID)
        {
            if (shoppingList.Length > 0)
            {
                for (int i = 0; i < shoppingList.Length; i++)
                {
                    if (shoppingList[i] == itemID && completionList[i] == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Randomizes the shopping list
        public void RandomiseList()
        {
            for (int i = 0; i < shoppingList.Length; i++)
            {
                shoppingList[i] = (int)Random.Range(0f, (float)System.Enum.GetValues(typeof(ItemID)).Length);
            }


            //DEBUG
            /*
            for (int l = 0; l < shoppingList.Length; l++)
            {
                print(l + ":  " + shoppingList[l]);
            }*/
        }


        //COMPLETION LIST

        //Updates the completion list with player's inventory
        public void UpdateCompletion()
        {
            ClearCompletion(); //Start from a clean slate

            bool[] itemFound = new bool[inventory.Length]; //Elements of this array will become 'true' if this item has already been used to check off a shopping list element
            for (int sl = 0; sl < shoppingList.Length; sl++) //Check every shopping list item
            {
                //Find the correct item in player inventory
                for (int inv = 0; inv < inventory.Length; inv++)
                {
                    if (inventory[inv] == shoppingList[sl] //Check if this inventory slot contains the correct item
                        && itemFound[inv] == false) //Make sure this item slot wasn't already marked as used
                    {
                        completionList[sl] = true; //This shopping list slot is complete
                        itemFound[inv] = true; //Mark this item slot as 'used'
                        break;
                    }
                }
            }
        }

        //Returns the number of completed shopping list items
        public int CountCompletion()
        {
            int count = 0;
            foreach (bool cl in completionList)
            {
                if (cl) { count++; }
            }

            return 0;
        }

        //Clears the completion list. Changes every value in the array to 'false'
        public void ClearCompletion()
        {
            for (int i = 0; i < completionList.Length; i++)
            { completionList[i] = false; }
        }




        //CASH


        public void RemoveCash(int amount)
        {

        }
    }
}
