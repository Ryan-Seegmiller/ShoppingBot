using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Elevator : MonoBehaviour
    {
        bool isOpen = false;

        public void DoorTrigger()
        {

        }
        public void Trigger()
        {
            Debug.Log("Elevator.Trigger() :: Elevator triggered at " + ((int)Time.time).ToString() + "s", this);
        }
    }
}
