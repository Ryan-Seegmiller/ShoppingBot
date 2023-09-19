using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //most vars will be made private later
    //locked on
    public bool hasFoundPlayer = false;
    //alerted
    public bool hasDetectedPlayer = false;
    public float detectionRadius =15;
    float time = 0;
    float firstDetectedTime=0;
    public float timeDetectionToFind = 5;
    Rigidbody rb;

    //Ai controls
    public float playerEnemyDifferenceY=0;
    public float pointAtPlayerChance =30;
    public Vector3 pointAtPlayerOffsetVector;
    public float pointAtPlayerOffset;

    public float yRotation = 0;
    public float yRotationReturn = 0.2f;
    public float yRotationPerArmDetection = 0.5f;

    public Transform rayPointArmRight;
    public Transform rayPointArmStraight;
    public Transform rayPointArmLeft;
    public float lrArmRange = 1;
    public float sArmRange = 1;
    public float wanderRotationLimits =0.2f;
    public float acceleration =15;
    public float maxSpeed = 5;
    public Vector2 reverseModifier = new Vector2(2, 3);
    public Vector2 stuckRotation = new Vector2(2, 3);
    //create vector3 from float
    public float PointAtPlayerOffset{
        set
        {
            pointAtPlayerOffset = value;
            pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        }
        get
        {
            return pointAtPlayerOffset;
        }
    }
    void Start()
    {
        GetComponent<SphereCollider>().radius = detectionRadius;
        pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //found player timer
        if (hasDetectedPlayer && !hasFoundPlayer && time>firstDetectedTime+ timeDetectionToFind)
        {
            hasFoundPlayer = true;
        }
        //clamp speed
        if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
        {
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));
            rb.velocity.Normalize();
        }
    }
    private void FixedUpdate()
    {
        DoMovement();
    }


    void DoMovement()
    {
        //yRotation represents how much to rotate this frame, not world rotation
        //smooth y rotation value back to 0.
        if (yRotation > 0)
            yRotation = Mathf.Clamp(yRotation - yRotationReturn, 0, 360);
        if (yRotation < 0)
            yRotation = Mathf.Clamp(yRotation + yRotationReturn, -360, 0);

        //calculations
        //arms/sensor rays
        Ray rr = new Ray(rayPointArmRight.transform.position, rayPointArmRight.transform.forward); // right
        Ray rl = new Ray(rayPointArmLeft.transform.position, rayPointArmLeft.transform.forward); // left
        Ray rs = new Ray(rayPointArmStraight.transform.position, rayPointArmStraight.transform.forward); // straight
        bool r = Physics.Raycast(rr, lrArmRange);
        bool l = Physics.Raycast(rl, lrArmRange);
        bool s = Physics.Raycast(rs, sArmRange);

        Debug.DrawRay(rr.origin, rr.direction * lrArmRange, Color.blue);
        Debug.DrawRay(rl.origin, rl.direction * lrArmRange, Color.red);
        Debug.DrawRay(rs.origin, rs.direction * sArmRange, Color.green);
        //wander, random rotation
        if (!hasFoundPlayer)
            yRotation += Random.Range(-wanderRotationLimits, wanderRotationLimits);

        //rotate if arms have collided
        if (r)
            yRotation += yRotationPerArmDetection;
        if (l)
            yRotation -= yRotationPerArmDetection;


        //if found player, track down player
        if (hasFoundPlayer)
        {
            if (Random.Range(0, 100) > pointAtPlayerChance)
            {
                //transform.eulerAngles = new Vector3(0, pointer.transform.eulerAngles.y, 0);
            }
        }
        //move forward / back up and rotate, depending on sensors
        if(!s && !(l&&r))
        {
            rb.AddForce(transform.forward * acceleration);
        }
        if((l&&r) || s)
        {
            rb.AddForce(-transform.forward * acceleration * Random.Range(reverseModifier.x,reverseModifier.y));
            yRotation += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1,2);

        }
        //set final rotation
        transform.Rotate(0, yRotation, 0);
    }


    //detect range to player, and wether or not player is 'seen'
    #region
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerControllerTest.instance.gameObject && !hasFoundPlayer && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true;
            firstDetectedTime = time;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerControllerTest.instance.gameObject)
        {
            firstDetectedTime = 0;
            hasFoundPlayer = false;
            hasDetectedPlayer = false;
        }
    }
    #endregion
}
