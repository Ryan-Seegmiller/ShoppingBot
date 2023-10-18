using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer beam;
    [SerializeField] private Transform muzzelPoint;
    [SerializeField] private GameObject armPivot;

    [SerializeField] private float maxLaserLength;
    [SerializeField] private float power;

    [SerializeField] private ParticleSystem hitParticleStystem;
    [SerializeField] private ParticleSystem muzzelParticleStystem;

    //Establishes if an object has been grabbed
    private ObjectGrab grab;
    private bool objectGrabbed;

    private void Awake()
    {
        beam.enabled = false;
        grab = GetComponent<ObjectGrab>();
    }
    private void Activate()
    {
        beam.enabled = true;
        if (!hitParticleStystem.isPlaying)
        {
            hitParticleStystem.Play();
        }
        if (!muzzelParticleStystem.isPlaying)
        {
            muzzelParticleStystem.Play();
        }
        
    }
    private void Deactivate()
    {
        beam.enabled = false;

        hitParticleStystem.Stop();
        muzzelParticleStystem.Stop();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            Activate();
        }
        else
        {
            Deactivate();

            beam.SetPosition(0, muzzelPoint.position);
            beam.SetPosition(1, muzzelPoint.position);
        }
    }
    private void FixedUpdate()
    {
        ArmLook();
        if (!beam.enabled) return;
        objectGrabbed = grab.ObjectDragActive;
        ShootLaser();
    }
    private void ShootLaser()
    {
        //Creates the ray cast and gets the position of the point hit
        Ray ray = new Ray(muzzelPoint.position, muzzelPoint.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLaserLength);
        Vector3 hitPosition = cast ? hit.point : muzzelPoint.position + muzzelPoint.forward * maxLaserLength;

        beam.SetPosition(0, muzzelPoint.position);
        beam.SetPosition(1, hitPosition);

        if (objectGrabbed)
        {
            grab.ResetObjectDrag();
        }
        HitDetection(cast, hit);
        hitParticleStystem.transform.position = hitPosition;
    }
    //Moves the arm twoards the mouse placement
    private void ArmLook()
    {
        //Points the arm to the crosshair placement
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLaserLength);
        

        //Gets if an object has been grabbed
        bool objectGrabbed = grab.ObjectDragActive;

        //Makes sure the arm is pointing as accuratley as possible
        if (cast && !objectGrabbed)
        {
            if (!(hit.collider.gameObject.name == "Rob Door"))

                armPivot.transform.LookAt(hit.point);
        }
        else if (!objectGrabbed)
        {
            armPivot.transform.forward = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        }
        else
        {
            armPivot.transform.LookAt(grab.currentObject.transform);
        }


    }
    //Adds force if theres an object in the path with a rigidbody
    private void HitDetection(bool cast, RaycastHit hit)
    {
        if (cast && hit.collider.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(armPivot.transform.forward * power/hit.distance, ForceMode.Force);            
        }
    }
}
