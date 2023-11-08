using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerContoller;
using System;

public class Laser : MonoBehaviour
{
    [Header("Laser stats")]
    [SerializeField] private float maxLaserLength;
    [SerializeField] private float power;

    [Header("Laser settings")]
    [SerializeField] private LineRenderer beam;
    [SerializeField] private Transform muzzelPoint;
    [SerializeField] private GameObject armPivot;

    [Header("Laser particles")]
    [SerializeField] private ParticleSystem hitParticleStystem;
    [SerializeField] private ParticleSystem muzzelParticleStystem;

    private ObjectGrab grab;

    //Establishes if an object has been grabbed
    private bool objectGrabbed;

    private void Awake()
    {
        beam.enabled = false;
        grab = GetComponent<ObjectGrab>();
    }
    private void Update()
    {   //Gets input
        InputCheck();
    }
    
    private void FixedUpdate()
    {
        ArmLook();
        if (!beam.enabled) return;
        objectGrabbed = grab.ObjectDragActive;
        ShootLaser();
    }
    private void InputCheck()
    {   //Gets if the player is holing down the mouse 0 button
        Action Shoot = (Input.GetMouseButton(0)) ? 
            () => { Activate(); } :
            () => { Deactivate(); SetLaserPosition(muzzelPoint.position);};
        Shoot();
    }
    private void ShootLaser()
    {
        //Creates the ray cast and gets the position of the point hit
        Ray ray = new Ray(muzzelPoint.position, muzzelPoint.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLaserLength);
        Vector3 hitPosition = cast ? hit.point : muzzelPoint.position + muzzelPoint.forward * maxLaserLength;

        SetLaserPosition(hitPosition);

        if (objectGrabbed)
        {
            grab.ResetObjectDrag();
        }
        HitDetection(cast, hit);
        hitParticleStystem.transform.position = hitPosition;
    }
    private void Activate()
    {
        //Plays particle system when shooting is active
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
        //Stops particle system when shooting is not active
        beam.enabled = false;
        hitParticleStystem.Stop();
        muzzelParticleStystem.Stop();
    }
    //Moves the arm twoards the mouse placement
    private void ArmLook()
    {
        //Points the arm to the crosshair placement
        Ray ray = Camera.main.ScreenPointToRay(PlayerMovement.mousePos);
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
            armPivot.transform.forward = Camera.main.ScreenPointToRay(PlayerMovement.mousePos).direction;
        }
        else
        {
            armPivot.transform.LookAt(grab.currentObject.transform);
        }


    }
    //Adds force if theres an object in the path with a rigidbody
    private void HitDetection(bool cast, RaycastHit hit)
    {
        if (cast && hit.collider.TryGetComponent(out EnemyBase eb))//Enemy damage
        {
            eb.Hit(Time.deltaTime);//Because its a constant beam, pass in TDT as a modifier to the damage value.
                                   //This way, it does 1 damage per second rather than 1 damage per frame/hit.
        }                          //currently its actually 3 damage per second. Determined in the EnemyBase.Hit()
        else if (cast && hit.collider.TryGetComponent(out Rigidbody rigidbody))
        {
            if(hit.collider.gameObject.layer == 11)
            {
                rigidbody.isKinematic = false;
            }
            rigidbody.AddForce(armPivot.transform.forward * power/hit.distance, ForceMode.Force);            
        }
    }
    private void SetLaserPosition(Vector3 endPos)
    {
        beam.SetPosition(0, muzzelPoint.position);
        beam.SetPosition(1, endPos);
    }
}
