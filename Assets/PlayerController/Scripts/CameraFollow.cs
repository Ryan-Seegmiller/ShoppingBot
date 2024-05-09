using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float distanceToFollowFrom;
    [SerializeField] private Transform followTarget;
    [HideInInspector] public float camAngle;
    

    private Camera cam;

    // Start is called before the first frame update
    private void Start()
    {
        SetCameraPosition();
        
    }
    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }
    public void SetCameraPosition()
    {
        //Sets how far the camera should be from the player and the angle
        GetCamera();
        Ray ray = new Ray(transform.position, -(transform.position - followTarget.position).normalized);
        if(Physics.Raycast(ray, out RaycastHit hit, distanceToFollowFrom))
        {
            print("hit");
            followTarget.position = hit.point;
        }
        else
        {
            followTarget.position = transform.position + (ray.direction * distanceToFollowFrom);
        }
        followTarget.LookAt(transform);
        cam.transform.rotation = Quaternion.Euler(new Vector3(camAngle, 0, 0));
        MoveCamera();
    }
    public void MoveCamera()
    {
        //Moves the camera to the follow position
        cam.transform.position = followTarget.position;
        //Looks at the player with the given cam angle
        cam.transform.LookAt(transform);
        Vector3 camEulers = cam.transform.rotation.eulerAngles;

        cam.transform.rotation = Quaternion.Euler(new Vector3(camAngle, camEulers.y, camEulers.z));
    }
    
    private void GetCamera()
    {
        //Sets the camera if there isnt one already
        if (cam == null)
        {
            cam = Camera.main;
        }
    }
    private void OnDrawGizmos()
    {
        GetCamera();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (transform.position + (transform.position - followTarget.position).normalized * -distanceToFollowFrom));
        Gizmos.color = Color.white;
        Gizmos.matrix = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, cam.transform.localScale);
        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, 200, 0, cam.aspect);
    }
}
