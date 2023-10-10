using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemInteract : MonoBehaviour
    {
        public int itemValue = -1;

        void Start()
        {

        }

        void Update()
        {
            //Destroy item if itemValue is -1
            if (itemValue == -1)
            {
                Destroy(gameObject);
            }
        }

        //Collect this item
        public void ItemCollect()
        {
            if (ItemManager.instance.CheckInventorySpace()) // Make sure inventory has space
            {
                ItemManager.instance.AddItem(itemValue);
                Destroy(gameObject);
            }
            else
            { //Inventory is full
                Debug.LogWarning("ItemIteract.ItemCollect() :: Trying to add an item when inventory is full.");
            }

        }
    }
}

