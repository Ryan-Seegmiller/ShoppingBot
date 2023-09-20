using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksFr : MonoBehaviour
{
    [SerializeField] int[] shopList = new int[] {1, 2, 3, 4};
    [SerializeField] string[] shopListNames;
    [SerializeField] int[] inventory;

    [SerializeField] int Index;

    private void Start()
    {
        shopListNames = new string[shopList.Length];
        inventory = new int[shopList.Length];

        for(int i = 0; i < shopListNames.Length; i++)
        {
            shopListNames[i] = "filler" + i;
        }
        for(int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = 0;
        }
    }

    public void SetSL()
    {
        //ShoppingList.instance.SetShopList(shopList, shopListNames);
    }
    public void Increment(int i)
    {
        if (inventory[i] < shopList[i])
        {
            inventory[i]++;
            //ShoppingList.instance.SetInv(inventory);
        }
    }
}
