//HOW TO ADD ITEMS
//Step 1: (In ItemEnums script) Add item in the 'itemID' enum. (Make sure it's positioned with other items in the same category)
//Step 2: (In ItemValue script) Add the cost of the item in the 'itemCost' array and comment the item name beside it.
//Step 3: (In Item gameObject) Add item model as a disabled child of the item gameObject, and then place the model into the itemModel array in the same position as the itemID.
//Step 4: (In Item gameObject) Give the item model a collider.
//NOTE: Step 2-4 must be organized in the same order as the 'itemID' array. Example: if 'strawberry' is 3rd in itemID (index 2), the strawberry's itemName, itemCost and itemModel must also be in the 3rd position (index 2)
//Step 5: (In ItemValue script) Adjust the int of the same category in the 'categorySize' array to match the number of items in that category

//HOW TO ADD ITEM CATEGORIES
//Step 1: (In ItemEnums script) Add a comment with the category name at the end of the 'itemID' array, and place all items belonging to this category after that comment
//Step 2: (In ItemEnums script) Add the category name into the 'itemCategory' enum
//Step 3: (In ItemValue script) Add a new int to the 'categorySize' array and comment which category that int belongs to. Int value should match the number of individual items in that category.
//NOTE: The category must be in the same position for all enums/arrays. If your new category is 4th in the 'itemID' enum, it should also be 4th in the'itemCategory' enum and 'categorySize' array 






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