using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameManager Instance;

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

        //Assign inventory and shopping list array size
        inventory = new int[inventorySize];
        shoppingList = new int[inventorySize];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
