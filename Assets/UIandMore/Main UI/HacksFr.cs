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
        if(shopList != null && shopList.Length <= GameManager.Instance.shoppingList.Length)
        {
            for(int i = 0; i < shopList.Length; i++)
            {
                GameManager.Instance.shoppingList[i] = shopList[i];
            }
        }
        for(int i = 0; i < GameManager.Instance.inventory.Length; i++)
        {
            GameManager.Instance.inventory[i] = -1;
        }
    }
    public void Increment()
    {
        GameManager.Instance.inventory[Index] = GameManager.Instance.shoppingList[Index];
    }

    public void diLength()
    {
        //print("di length: " + ShoppingList.instance.displayItems.Length);
        //print(GameManager.Instance.shoppingList[Index]);
        //TimeCalc.instance.SetTimer(599900);
        print(ScoreCalc.instance.GetScore() + "0 Points!");
    }
    public void diCheck()
    {
        GameManager.Instance.AddItem(ItemToAdd);
    }
}
