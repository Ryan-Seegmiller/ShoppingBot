using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectGrab : MonoBehaviour
{
    [Header("Object References")]
    public Image crosshair;
    [SerializeField] private Camera mainCamera;//main camera
    public GameObject target;//Object in place for the item to move to
    public GameObject armPivot;

    [Header("Item layer")]
    public LayerMask layersToHit;

    public float objectToGrabDistance = 10f;

    //Positions for object drag
    private Vector3 worldPosition;
    private Vector3 pullPosition;

    //Rigidbodys
    Rigidbody rbItem;
    Rigidbody rb;

    //Raycast varibles
    [NonSerialized]public RaycastHit currentObject;
    private RaycastHit raycastHit;
    private RaycastHit EmptyRaycastHit;

    Ray rayLook;

    //Mouse positions
    float mZCoord;
    Vector3 mousePos;

    //Bools
    [Header("Toggle drag")]
    public bool draggingActive = false;
    [NonSerialized] public bool ObjectDragActive = false;



    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerInput();

        //crosshair placement
        crosshair.transform.position = Input.mousePosition;

        rayLook = mainCamera.ScreenPointToRay(Input.mousePosition);

        

        if (Physics.Raycast(rayLook,out raycastHit, objectToGrabDistance, layersToHit))
        {
            //print(raycastHit.collider.gameObject.name);
            Debug.DrawRay(rayLook.origin, rayLook.direction * 300, Color.red);
            
        }
        if (!draggingActive)
        {
            ObjectDrag();
        }
        if (!ObjectDragActive)
        {
            //Arm look rotiation reset
            armPivot.transform.rotation = Quaternion.Lerp(armPivot.transform.rotation, gameObject.transform.rotation, .1f);
        }
        
        
    }
    
    private void PlayerInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!ObjectDragActive)
            {
                ToggleDrag();
            }
            ;
           if(ObjectDragActive)
            {
                //throws the item
                //ThrowObject();
            }
            //Sets the object drag mode
            ResetObjectDrag();
        }
        else if (Input.GetMouseButton(1) && draggingActive) 
        {
            ObjectDrag();
        }
        if(Input.GetMouseButtonUp(1) && ObjectDragActive && draggingActive)
        {
            ResetObjectDrag();
        }
        if (Input.mouseScrollDelta != new Vector2(0,0) && ObjectDragActive)
        {
            ObjectPull();
        }
    }
    public void ResetObjectDrag()
    {
        //ThrowObject();
        ObjectDragActive = (!ObjectDragActive && (raycastHit.collider != null || raycastHit.collider != currentObject.collider)) ? true : false;
    }
    private void ObjectPull()
    {        
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

            //Arm look rotiation
            armPivot.transform.LookAt(currentObject.transform);
        }
        else
        {
            currentObject = EmptyRaycastHit;
        }
    }
    private void ToggleDrag()
    {
        if(raycastHit.collider != null)
        {
            currentObject = raycastHit;//Sets the current object to the one clicked
            rbItem = currentObject.transform.GetComponent<Rigidbody>();//Gets the rigidbody of the object grabbed

            target.transform.position = currentObject.transform.position;//sets the objects position

            mZCoord = mainCamera.WorldToScreenPoint(target.transform.position).z;//Sets the z axis for the object
            pullPosition = Vector3.zero;
        }
    }
    private void ThrowObject()
    {
        //throws the item
        if(rbItem != null)
        {
            rbItem.AddForce(mainCamera.transform.forward + rayLook.direction * 50f, ForceMode.Impulse);
        }
    }
}
