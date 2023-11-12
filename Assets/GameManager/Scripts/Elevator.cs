using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerContoller;

public class Elevator : MonoBehaviour
{
    public Collider lockCollider;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;
    [SerializeField, Range(0, 1f)] float openPercent = 0;

    internal void LockElevator()
    {
        lockCollider.enabled = true;
    }
    internal void UnlockElevator()
    {
        lockCollider.enabled = false;
    }
    private void UpdateElevatorDoors()
    {
        openPercent = Mathf.Clamp01(openPercent);
        leftDoor.localPosition = new Vector3(Mathf.Lerp(0, 1, openPercent), 0, 0);
        rightDoor.localPosition = new Vector3(Mathf.Lerp(0, -1, openPercent), 0, 0);
    }

    private void Update()
    {
        float direction = (lockCollider.enabled) ? -1 : 1;
        openPercent = openPercent + (Time.deltaTime * direction);
        UpdateElevatorDoors();
    }
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
    private void OnValidate()
    {
        if (leftDoor == null || rightDoor == null) { Debug.LogWarning($"Elevator :: null reference to elevator doors.", this); return; }
        UpdateElevatorDoors();
    }
}
