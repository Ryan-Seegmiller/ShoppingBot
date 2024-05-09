using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerContoller;
using UnityEngine.UIElements;

public class ObjectGrab : MonoBehaviour
{
    [Header("Object References")]
    private Camera mainCamera;//main camera
    public GameObject target;//Object in place for the item to move to
    public GameObject armPivot;
    public Transform cartColliderTR;

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
    [NonSerialized]public RaycastHit currentHeldObject;
    private RaycastHit raycastHit;
    private RaycastHit emptyRaycastHit;

    Ray rayLook;

    //Mouse positions
    float mZCoord;

    //Timer
    private WaitForSeconds colReset = new WaitForSeconds(1f);

    //Item Collection
    [NonSerialized] public bool canCollect;
    

    //Bools
    [Header("Toggle drag")]
    [NonSerialized] public bool ObjectDragActive = false;
    bool snapped;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (PlayerMovement.isPaused) { return; }
        PlayerInput();

        rayLook = mainCamera.ScreenPointToRay(PlayerMovement.mousePos);
        
        ObjectDrag();

        if (!ObjectDragActive && !Physics.Raycast(rayLook, out raycastHit, objectToGrabDistance, layersToHit))
        {
            //Arm look rotiation reset
            armPivot.transform.rotation = Quaternion.Lerp(armPivot.transform.rotation, gameObject.transform.rotation, .1f);
        }
       
        ObjectSnapping();

    }
    private void PlayerInput()
    {
        bool initialMouseButtonDown = Input.GetMouseButtonDown(1) && !ObjectDragActive;
        if (initialMouseButtonDown)
        {
            ToggleDrag();
            ResetObjectDrag();
        }
      
        if (Input.GetMouseButton(1)) 
        {
            ObjectDrag();
            ItemDisableCollison();
        }
        if(Input.GetMouseButtonUp(1) && ObjectDragActive)
        {
            ObjectEndGrab();
        }
        if (Input.mouseScrollDelta != new Vector2(0,0))
        {
            ObjectPull();
        }
    }
    public void ObjectEndGrab()
    {
        StartCoroutine(ItemEnableCollison());
        ResetObjectDrag();
        currentHeldObject = emptyRaycastHit;
    }
    public void ResetObjectDrag()
    {
        //ThrowObject();
        ObjectDragActive = (!ObjectDragActive && (raycastHit.collider != null || raycastHit.collider != currentHeldObject.collider)) ? true : false;
    }
    private void ObjectPull()
    {
        if (!ObjectDragActive) { return; }

        //Move sthe object to and from the camera using a raycaster as a guide
       
        pullPosition += new Vector3(Mathf.Abs(rayLook.direction.x), Mathf.Abs(rayLook.direction.y), Mathf.Abs(rayLook.direction.z)) * Input.mouseScrollDelta.y;
        
    }

    private void ObjectDrag()
    {   //Returns if object drag is not active or if there is no collider 
        if(!ObjectDragActive && currentHeldObject.collider == null) {return;}

        //Sets the object postition
        worldPosition = new Vector3(PlayerMovement.mousePos.x + currentHeldObject.transform.position.z, PlayerMovement.mousePos.y+currentHeldObject.transform.position.z, -mainCamera.transform.position.z + currentHeldObject.transform.position.z);
        Vector3 objectPosition = (mainCamera.ScreenToWorldPoint(worldPosition) - currentHeldObject.transform.position);

        PlayerMovement.mousePos.z = mZCoord;//Sets the mouse position on the z

        //Translates the the object to be pulled to to the mouse position in the world
        target.transform.position = (!snapped)? mainCamera.ScreenToWorldPoint(PlayerMovement.mousePos + pullPosition) + rb.velocity.normalized : target.transform.position;

        //Move object towards the object that the camera creates
        rbItem.velocity = (target.transform.position - currentHeldObject.transform.position) * objectPosition.magnitude * 4;

        armPivot.transform.LookAt(currentHeldObject.transform);//Targets the arm onto the object
        
    }
    private void ToggleDrag()
    {   //Returns if no object is being grabbed
        if(raycastHit.collider == null) {return;}
        
        currentHeldObject = raycastHit;//Sets the current object to the one clicked
        rbItem = currentHeldObject.transform.GetComponent<Rigidbody>();//Gets the rigidbody of the object grabbed
        
        //Enables item physics
        EnableItemPhysics();

        target.transform.position = currentHeldObject.transform.position;//sets the objects position

        mZCoord = mainCamera.WorldToScreenPoint(target.transform.position).z;//Sets the z axis for the object
        pullPosition = Vector3.zero;
        
    }
    private void ItemDisableCollison()
    {   if(currentHeldObject.collider == null) { return; }
        //Disable collison with player of item grabbed
        Transform[] objChildren = currentHeldObject.collider.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform tr in objChildren)
        {
            if(tr.gameObject.layer == 13)
            {
                tr.gameObject.GetComponent<Collider>().enabled = false;
                break;
            }
        }
    }
   
    IEnumerator ItemEnableCollison()
    {   //Enable collison with player of item grabbed
        RaycastHit raycastHit = currentHeldObject;
        canCollect = true;
        yield return colReset;
        canCollect = false;

        if (raycastHit.collider != null)
        {
            Transform[] objChildren = raycastHit.collider.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform tr in objChildren)
            {
                if (tr.gameObject.layer == 13)
                {
                    tr.gameObject.GetComponent<Collider>().enabled = true;
                    break;
                }
            }
        }
    }
    private void EnableItemPhysics()
    {
        ItemRender objRender = currentHeldObject.transform.GetComponent<ItemRender>(); //Get the grabbed object's ItemRender script

        objRender.EnablePhysics(); //Enable grabbed object's physics
    }
    private void ObjectSnapping()
    {
        if(currentHeldObject.collider == null) {return; }
        if(Vector3.Distance(cartColliderTR.position, currentHeldObject.transform.position) < 2f && ObjectDragActive)
        {
            target.transform.position = cartColliderTR.position + Vector3.up;
            snapped = true;
            return;
        }
        snapped = false;
        
    }
}
