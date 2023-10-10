using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using items;

public class ItemInteract : MonoBehaviour
{
    ItemValue itemValue;

    void Start()
    {
        //Get ItemValue class for this item
        itemValue = GetComponent<ItemValue>();
    }

    void Update()
    {
        //DEBEUG
        if (Input.GetKeyDown(KeyCode.O))
        {
            //ItemCollect();
            GameManager.Instance.RandomiseList();
        }
    }

    //Collect this item
    /*
    public void ItemCollect()
    {/*
        if (GameManager.Instance != null && itemValue != null)//Make sure GameManager and ItemValue exist
        { if (GameManager.Instance.CheckInventorySpace()) // Make sure inventory has space
            {
                GameManager.Instance.AddItem(itemValue.id);
            } else { //Inventory is full
                print("ERROR: Trying to add an item when inventory is full.");
            }
        }*/
        
    }

