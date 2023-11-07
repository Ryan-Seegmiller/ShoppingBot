using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerContoller;

public class Elevator : MonoBehaviour
{
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
}
