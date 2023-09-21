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

    Rigidbody rb;


    private RaycastHit currentObject;

    private RaycastHit raycastHit;
    private RaycastHit EmptyRaycastHit;
    public bool ObjectDragActive = false;

    public float maxObjectSpeed = 20f;
    public float maxDistance = 10f;
    public float minDistance = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

            ObjectDragActive = (!ObjectDragActive && (raycastHit.collider != null || raycastHit.collider != currentObject.collider)) ? true : false;
            if(raycastHit.collider != null)
            {
                currentObject = raycastHit;
            }
        }
    }
    private void ObjectDrag()
    {
        if(ObjectDragActive && currentObject.collider != null)
        {
            worldPosition = new Vector3(Input.mousePosition.x + currentObject.transform.position.z, Input.mousePosition.y+currentObject.transform.position.z, -mainCamera.transform.position.z + currentObject.transform.position.z);
            Vector3 objectPosition = (mainCamera.ScreenToWorldPoint(worldPosition) - currentObject.transform.position);
            Rigidbody rbObject = currentObject.transform.GetComponent<Rigidbody>();
            //currentObject.transform.position = new Vector3(Mathf.Clamp(currentObject.transform.position.x, minDistance, maxDistance), Mathf.Clamp(currentObject.transform.position.y, minDistance, maxDistance), Mathf.Clamp(currentObject.transform.position.z, minDistance, maxDistance));
            
            Vector3 distance = mainCamera.transform.position - currentObject.transform.position;
            Vector3 distanceToCursor = mainCamera.ScreenToWorldPoint(worldPosition) - currentObject.transform.position;
            //print(objectPosition);
            //print(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            //print(distanceToCursor.magnitude);
            //print(distance.magnitude);
            if(!(distance.magnitude >= maxDistance) && !(distance.magnitude <= minDistance))
            {
                rbObject.isKinematic = false;
                rbObject.velocity = ((objectPosition.normalized * objectPosition.magnitude * 20)) + rb.velocity;
            }
            else
            {
                print(distanceToCursor.magnitude);
                rbObject.velocity = ((objectPosition.normalized * objectPosition.magnitude * 20) ) + rb.velocity;
            }

            
            /*
            if (rb.velocity.magnitude > maxObjectSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxObjectSpeed;
            }*/
        }
        else
        {
            currentObject = EmptyRaycastHit;
        }
    }
    
}
