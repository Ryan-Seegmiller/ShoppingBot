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
    public Vector3 worldPosition;

    Rigidbody rbItem;


    private RaycastHit currentObject;

    private RaycastHit raycastHit;
    private RaycastHit EmptyRaycastHit;

    float mZCoord;
    Vector3 mousePos;

    public bool ObjectDragActive = false;

    
    void Start()
    {
    }

    void Update()
    {
        PlayerInput();

        //crosshair placement
        Image.transform.position = Input.mousePosition;

        Ray rayLook = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayLook,out raycastHit, 10, layersToHit))
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
                mousePos.z = mZCoord;//Sets the mouse pos axis for the z

            }
            else if(ObjectDragActive)
            {
                //throws the item
                rbItem.AddForce(mainCamera.transform.forward * 10f, ForceMode.Impulse);
                
            }
            ObjectDragActive = (!ObjectDragActive && (raycastHit.collider != null || raycastHit.collider != currentObject.collider)) ? true : false;

        }
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
            target.transform.position = mainCamera.ScreenToWorldPoint(mousePos);

            //Move object towards the object that the camera creates
            rbItem.velocity = (target.transform.position - currentObject.transform.position) * objectPosition.magnitude;
        }
        else
        {
            currentObject = EmptyRaycastHit;
        }
    }
    
}
