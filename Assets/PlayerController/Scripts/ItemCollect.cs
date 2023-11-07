using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    private bool canCollect;
    private void OnTriggerEnter(Collider other)
    {
        canCollect = GetComponentInParent<ObjectGrab>().canCollect;
        if (!canCollect) { return; }
        if(other == null) { return; }
        other.gameObject.GetComponentInParent<Transform>().GetComponent<ItemInteract>().ItemCollect();
        
    }
}
