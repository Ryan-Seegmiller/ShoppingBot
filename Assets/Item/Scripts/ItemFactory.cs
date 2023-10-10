using UnityEngine;
using System;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Item Factory")]


    public class ItemFactory : ScriptableObject
    {
        [SerializeField] GameObject[] items;
        public GameObject InstanceItem<T>(T id, Vector3 pos)
        {
            //Create item using item ID
            if (id.GetType().Equals(typeof(ItemID)))
            {
                //Convert 'id' to an int
                System.Object o = id;
                int n = (int)o;

                //Instantiate object
                return Instantiate(items[n], pos, Quaternion.identity);
            }
            //Create random item within a given category
            else if (id.GetType() == typeof(ItemCategory))
            {
                //Convert 'id' to an int
                System.Object o = id;
                int n = (int)o;

                int indexCount = 0; //Get the number of items in the itemID enum before the chosen category starts
                for (int i = 0; i < ItemValue.Instance.categorySize.Length - (ItemValue.Instance.categorySize.Length - n); i++)
                { //Repeat for each category before the chosen category
                    Debug.Log(i);
                    indexCount += ItemValue.Instance.categorySize[i];
                }
                int itemIdMin = n > 0 ? indexCount : 0; //The item ID of the first available item in the chosen category
                int itemIdMax = indexCount + ItemValue.Instance.categorySize[n] - 1; //The item ID of the last available item in the chosen category
                int newChoice = Mathf.RoundToInt(UnityEngine.Random.Range((float)itemIdMin - 0.5f, (float)itemIdMax + 0.4f)); //Choose which item ID to assign the item
                                                                                                                              //Assign item to new item choice
                return Instantiate(items[newChoice], pos, Quaternion.identity);
            }
            //Create a random item
            else
            {
                return null;
            }
        }
    }
}