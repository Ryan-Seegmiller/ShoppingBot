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

    void FixedUpdate()
    {
        Image.transform.position = Input.mousePosition;
        Ray rayLook = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayLook, out raycastHit, 10, layersToHit))
        {
            Debug.DrawRay(mainCamera.transform.up, rayLook.direction * 300, Color.red);
            
        }
        ObjectDrag();
        
    }
    private void Update()
    {
        PlayerInput();
    }
    private void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            ObjectDragActive = (!ObjectDragActive && raycastHit.collider != null || raycastHit.collider != currentObject.collider) ? true : false;
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
            worldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z + currentObject.transform.position.z);
            Vector3 objectPosition = (mainCamera.ScreenToWorldPoint(worldPosition) - currentObject.transform.position).normalized;
            Rigidbody rbObject = currentObject.transform.GetComponent<Rigidbody>();

            Vector3 distance = mainCamera.transform.position - currentObject.transform.position;
            if(!(distance.magnitude >= maxDistance) && !(distance.magnitude <= minDistance))
            {
                rbObject.velocity = (objectPosition.normalized * objectPosition.magnitude *  10) + rb.velocity;
            }
            else
            {
                rbObject.velocity = rb.velocity;
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
