using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Image Image;
    [SerializeField] private Camera mainCamera;
    public GameObject target;
    public LayerMask layersToHit;

    public float objectToGrabDistance = 10f;

    private Vector3 worldPosition;

    Rigidbody rbItem;
    Rigidbody rb;


    private RaycastHit currentObject;

    private RaycastHit raycastHit;
    private RaycastHit EmptyRaycastHit;

    float mZCoord;
    Vector3 mousePos;

    public bool ObjectDragActive = false;
    private Vector3 pullPosition;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerInput();

        //crosshair placement
        Image.transform.position = Input.mousePosition;

        Ray rayLook = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayLook,out raycastHit, objectToGrabDistance, layersToHit))
        {
            //print(raycastHit.collider.gameObject.name);
            Debug.DrawRay(rayLook.origin, rayLook.direction * 300, Color.red);
            
        }
        ObjectDrag();
        
    }
    
    private void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if(raycastHit.collider != null && !ObjectDragActive)
            {
                currentObject = raycastHit;//Sets the current object to the one clicked
                rbItem = currentObject.transform.GetComponent<Rigidbody>();//Gets the rigidbody of the object grabbed

                target.transform.position = currentObject.transform.position;//sets the objects position
                
                mZCoord = mainCamera.WorldToScreenPoint(target.transform.position).z;//Sets the z axis for the object
                pullPosition = Vector3.zero;

            }
            else if(ObjectDragActive)
            {
                Ray rayLook = mainCamera.ScreenPointToRay(Input.mousePosition);

                //throws the item
                rbItem.AddForce(mainCamera.transform.forward + rayLook.direction * 50f, ForceMode.Impulse);
                
            }
            //Sets the object drag mode
            ObjectDragActive = (!ObjectDragActive && (raycastHit.collider != null || raycastHit.collider != currentObject.collider)) ? true : false;

        }
        if(Input.mouseScrollDelta != new Vector2(0,0) && ObjectDragActive)
        {
            ObjectPull();
        }
    }

    private void ObjectPull()
    {
        Ray rayLook = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        //Move sthe object to and from the camera using a raycaster as a guide
        pullPosition = Vector3.ClampMagnitude(pullPosition, objectToGrabDistance);
        pullPosition += new Vector3(Mathf.Abs(rayLook.direction.x), Mathf.Abs(rayLook.direction.y), Mathf.Abs(rayLook.direction.z)) * Input.mouseScrollDelta.y;
        
    }

    private void ObjectDrag()
    {
        if(ObjectDragActive && currentObject.collider != null)
        {
            worldPosition = new Vector3(Input.mousePosition.x + currentObject.transform.position.z, Input.mousePosition.y+currentObject.transform.position.z, -mainCamera.transform.position.z + currentObject.transform.position.z);
            Vector3 objectPosition = (mainCamera.ScreenToWorldPoint(worldPosition) - currentObject.transform.position);

            //Sets the mouse position
            mousePos = Input.mousePosition;
            mousePos.z = mZCoord;
            //Translates the the object to be pulled to to the mouse position in the world
            target.transform.position = mainCamera.ScreenToWorldPoint(mousePos + pullPosition) + rb.velocity.normalized;

            //Move object towards the object that the camera creates
            rbItem.velocity = (target.transform.position - currentObject.transform.position) * objectPosition.magnitude;
        }
        else
        {
            currentObject = EmptyRaycastHit;
        }
    }
    private void OnMouseDrag()
    {
        ObjectDrag();
    }

}
