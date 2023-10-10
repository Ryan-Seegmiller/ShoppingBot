using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    //Item id enum
    public enum itemID
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
    public enum itemCategory
    {
        food,
        clothes,
        tools
    };

    public class ItemValue : MonoBehaviour
    {

        public static ItemValue Instance;

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


        void Start()
        {
            //Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

    }
}
