using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Elevator : MonoBehaviour
    {
        public void Trigger()
        {
            Debug.Log("Elevator.Trigger() :: Elevator triggered at " + ((int)Time.time).ToString() + "s", this);
        }
    }
}
