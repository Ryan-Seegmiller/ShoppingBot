using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksFr : MonoBehaviour
{
    [SerializeField] int[] shopList;
    [SerializeField] string[] shopListNames;
    [SerializeField] int[] inventory;

    [SerializeField] int Index;

    [SerializeField] int ItemToAdd;

    public void SetSL()
    {
        if(shopList != null && shopList.Length <= ItemManager.instance.shoppingList.Length)
        {
            for(int i = 0; i < shopList.Length; i++)
            {
                ItemManager.instance.shoppingList[i] = shopList[i];
            }
        }
        for(int i = 0; i < ItemManager.instance.inventory.Length; i++)
        {
            ItemManager.instance.inventory[i] = -1;
        }
    }
    public void Increment()
    {
        ItemManager.instance.inventory[Index] = ItemManager.instance.shoppingList[Index];
    }

    public void diLength()
    {
        //print("di length: " + ShoppingList.instance.displayItems.Length);
        //print(GameManager.Instance.shoppingList[Index]);
        //TimeCalc.instance.SetTimer(599900);
        print(ScoreCalc.instance.GetScore() + " Points!");
    }
    public void diCheck()
    {
        ItemManager.instance.AddItem(ItemToAdd);
    }
}
