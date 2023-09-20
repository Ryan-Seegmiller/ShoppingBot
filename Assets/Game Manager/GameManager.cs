using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //PLAYER STATS
    public int cash = 100; //Player cash
    public int inventorySize = 10; //Inventory size
    //Player inventory
    [HideInInspector] public int[] inventory;
    //Shopping list
    [HideInInspector] public int[] shoppingList;



    void Start()
    {
        //Singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this); }

        //Assign inventory and shopping list array size, and set each item to -1
        inventory = new int[inventorySize];
        for (int i = 0; i < inventory.Length; i++) {
            inventory[i] = -1; //Assign every item to -1
        }
        shoppingList = new int[inventorySize];
        for (int i = 0; i < inventory.Length; i++) {
            shoppingList[i] = -1; //Assign every item to -1
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public string ItemName(int itemID)
    {
        //Assign name
        string nameLower = ((itemID)itemID).ToString();
        return char.ToUpper(nameLower[0]) + nameLower.Substring(1);
    }


    //Adds an item to the first empty item slot (-1) in the inventory array
    public void AddItem(int itemID)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == -1) //Check if index is available
            {
                inventory[i] = itemID; //Add item to the available index
                break;
            }
        }

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


    public void RandomiseList()
    {
        for (int i = 0; i < shoppingList.Length; i++)
        {
            //shoppingList[i] = (int)Random.Range((float), (float));
        }


        //DEBUG
        for (int l = 0; l < shoppingList.Length; l++)
        {
            print(l + ":  " + shoppingList[l]);
        }
    }
}
