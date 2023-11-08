using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerContoller;

public class Elevator : MonoBehaviour
{
    public Collider lockCollider;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponentInChildren<PlayerMovement>())
        {
            GameManager.instance.OnPlayerEnterElevator();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.GetComponentInChildren<PlayerMovement>())
        {
            GameManager.instance.OnPlayerExitElevator();
        }
    }

    internal void LockElevator()
    {
        lockCollider.enabled = true;
    }
    internal void UnlockElevator()
    {
        lockCollider.enabled = false;
    }
}
