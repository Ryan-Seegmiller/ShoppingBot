using UnityEngine;
using audio;

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

        //Collect this item
        public void ItemCollect()
        {
            if (ItemManager.instance.CheckInventorySpace() && ItemManager.instance.ListNeedsItem(itemValue)) // Make sure inventory has space
            {
                ItemManager.instance.AddItem(itemValue);
                //Play sound
                AudioManager.instance.PlaySound2D(5);
                //Destroy item
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
            AudioManager.instance.PlaySound2D(3);
            Destroy(gameObject);
        }
    }
}

