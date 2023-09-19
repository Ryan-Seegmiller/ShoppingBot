using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //most vars will be made private later
    //locked on
    [SerializeField]
    private bool hasFoundPlayer = false;
    //alerted
    [SerializeField]
    private bool hasDetectedPlayer = false;

    private float time = 0;
    private float firstDetectedTime =0;

    private Rigidbody rb;
    //Ai controls
    [SerializeField]
    private bool isFlying;
    [SerializeField]
    private float pointAtPlayerOffset;
    [SerializeField]
    private float detectionRadius = 15;
    [SerializeField]
    private float timeDetectionToFind = 5;
    [SerializeField]
    private float pointAtPlayerChance =30;

    private Vector3 pointAtPlayerOffsetVector;


    private float yRotation = 0;
    [Header("Rotation Smoothing")]
    [SerializeField]
    private float yRotationReturn = 0.2f;
    [SerializeField]
    private float yRotationPerArmDetection = 0.5f;
    [SerializeField]
    [Header("Rays")]
    private Transform rayPointArmRight;
    [SerializeField]
    private Transform rayPointArmLeft;
    [SerializeField]
    private float lrArmRange = 1;
    [SerializeField]
    private float lrRayAngle = 35;
    [SerializeField]
    private Transform rayPointArmStraight;
    [SerializeField]
    private float sArmRange = 1;
    [Header("Wander")]
    [SerializeField]
    private float wanderRotationLimits =0.2f;
    [SerializeField]
    private float wanderForceLimits =5;
    [Header("Speed Controls")]
    [SerializeField]
    private float acceleration =15;
    [SerializeField]
    private Vector2 reverseModifier = new Vector2(2, 3);
    [SerializeField]
    private Vector2 stuckRotation = new Vector2(2, 3);
    public bool RandomValues = false;
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
        if (RandomValues)
            GetRandomAIValues();
        if (isFlying)
            transform.position = new Vector3(transform.position.x, EnemyManager.instance.aerialEnemyHeight, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, EnemyManager.instance.groundEnemyHeight, transform.position.z);

        GetComponent<SphereCollider>().radius = detectionRadius;
        pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rayPointArmLeft.localEulerAngles = new Vector3(0, -lrRayAngle, 0);
        rayPointArmRight.localEulerAngles = new Vector3(0, lrRayAngle, 0);
        time += Time.deltaTime;
        //found player timer
        if (hasDetectedPlayer && !hasFoundPlayer && time>firstDetectedTime+ timeDetectionToFind)
        {
            hasFoundPlayer = true;
        }
    }
    private void FixedUpdate()
    {
        if(!isFlying)
            DoGroundAI();
        else
            DoAerialAI();  
    }


    void DoGroundAI()
    {
        //DROP TO 0
        if (yRotation > 0)
            yRotation = Mathf.Clamp(yRotation - yRotationReturn, 0, 360);
        if (yRotation < 0)
            yRotation = Mathf.Clamp(yRotation + yRotationReturn, -360, 0);

        //WANDER
        if (!hasFoundPlayer)
        {
            yRotation += Random.Range(-wanderRotationLimits, wanderRotationLimits);
        }

        //PLAYER HUNT BEHAVIOR
        //if found player, and not flying, track down player
        if (hasFoundPlayer)
        {
            if (Random.Range(0, 100) > pointAtPlayerChance)
            {
                //transform.eulerAngles = new Vector3(0, pointer.transform.eulerAngles.y, 0);
            }
        }

        //SENSORS
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
        //rotate if arms have collided
        if (r)
            yRotation += yRotationPerArmDetection;
        if (l)
            yRotation -= yRotationPerArmDetection;

        //move forward / back up and rotate, depending on sensors
        if (!s && !(l&&r))
        {
            //proceed forward
            transform.Translate(transform.forward * acceleration);
        }
        if((l&&r) || s)
        {
            //backup if stuck
            transform.Translate(-transform.forward * acceleration * Random.Range(reverseModifier.x,reverseModifier.y));
            yRotation += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1,2);
        }
        //set final rotation
        transform.Rotate(0, yRotation, 0);
    }

    void DoAerialAI()
    {
        //DROP TO 0
        if (yRotation > 0)
            yRotation = Mathf.Clamp(yRotation - yRotationReturn, 0, 360);
        if (yRotation < 0)
            yRotation = Mathf.Clamp(yRotation + yRotationReturn, -360, 0);

        //WANDER
        //transform.Translate(Vector3.right * acceleration * Random.Range(-wanderForceLimits, wanderForceLimits));
        


        //PLAYER HUNT BEHAVIOR

        //when flying, if close to player, lerp y offset.
        if (isFlying)
        {
            float offset = 0;
            if (hasFoundPlayer)
            {
                float dist = Mathf.Abs(Vector3.Distance(transform.position, PlayerControllerTest.instance.transform.position));
                offset = Mathf.Clamp(Map(detectionRadius - dist, dist, detectionRadius, 0, EnemyManager.instance.aerialEnemyHeight - 1), 0, EnemyManager.instance.aerialEnemyHeight - 1);
            }
            transform.position = new Vector3(transform.position.x, EnemyManager.instance.aerialEnemyHeight - offset, transform.position.z);
        }


        //SENSORS
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

        //rotate if arms have collided
        if (r)
            yRotation += yRotationPerArmDetection;
        if (l)
            yRotation -= yRotationPerArmDetection;

        //move forward / back up and rotate, depending on sensors
        if (!s && !(l && r))
        {
            //proceed forward
            transform.Translate(transform.forward * acceleration);
        }
        if ((l && r) || s)
        {
            //backup if stuck
            transform.Translate(-transform.forward * acceleration * Random.Range(reverseModifier.x, reverseModifier.y));
            yRotation += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
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
    public static float Map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }

    public void GetRandomAIValues()
    {
        lrArmRange = Random.Range(0.5f, 5f);
        sArmRange = Random.Range(0.5f, 5f);
        lrRayAngle = Random.Range(5, 85f);

        detectionRadius = Random.Range(3, 8f);
        wanderRotationLimits = Random.Range(0.001f, 0.5f);
        wanderForceLimits = Random.Range(0.001f, 2f);

        yRotationPerArmDetection = Random.Range(0.001f, 0.1f);
        yRotationReturn = yRotationPerArmDetection+Random.Range(0.001f, 0.1f);

        pointAtPlayerChance = Random.Range(1, 90f);
        timeDetectionToFind = Random.Range(1, 10f);

        acceleration = Random.Range(0.01f, 0.5f);

        reverseModifier = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.1f, 0.2f));
        stuckRotation = new Vector2(Random.Range(0.01f, 0.2f), Random.Range(0.2f, 1f));
    }
}
