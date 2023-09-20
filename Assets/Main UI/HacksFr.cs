using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksFr : MonoBehaviour
{
    [SerializeField] int[] shopList;
    [SerializeField] string[] shopListNames;
    [SerializeField] int[] inventory;

    [SerializeField] int Index;

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
            GameManager.Instance.inventory[i] = 0;
        }
    }
    public void Increment()
    {
        GameManager.Instance.inventory[Index]++;
    }
}
