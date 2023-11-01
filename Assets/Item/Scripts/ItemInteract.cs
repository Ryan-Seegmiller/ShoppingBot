using UnityEngine;

namespace Items
{
    public class ItemInteract : MonoBehaviour
    {
        public int itemValue = -1;

        private Rigidbody rb;

        void Start()
        {
            //Get rigidbody
            rb = GetComponent<Rigidbody>();
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
            if (ItemManager.instance.CheckInventorySpace() && ItemManager.instance.ListNeedsItem(itemValue)) // Make sure inventory has space
            {
                ItemManager.instance.AddItem(itemValue);
                Destroy(gameObject);
            }
            else
            { //Inventory is full
                ItemReject();
            }
        }



        public void ItemReject()
        {
            //rb.AddExplosionForce(10, player center, 3)
        }
    }
}

