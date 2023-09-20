using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksFr : MonoBehaviour
{
    [SerializeField] GameManager gameM;

    [SerializeField] int[] shopList;
    [SerializeField] string[] shopListNames;
    [SerializeField] int[] inventory;

    [SerializeField] int Index;

    private void Start()
    {
        
    }

    public void SetSL()
    {
        if(shopList != null && shopList.Length <= gameM.shoppingList.Length)
        {
            for(int i = 0; i < shopList.Length; i++)
            {
                gameM.shoppingList[i] = shopList[i];
            }
        }
    }
    public void Increment()
    {
        //might or might not be necessary
        if(gameM.inventory[Index] != null)
        {
            gameM.inventory[Index]++;
        }
    }
}
