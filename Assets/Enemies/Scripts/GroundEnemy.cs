using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    public int roamMode = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (RandomValues)
            GetRandomAIValues();
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
        if (hasDetectedPlayer && !hasFoundPlayer && time > firstDetectedTime + timeDetectionToFind)
        {
            hasFoundPlayer = true;
        }
    }
    private void FixedUpdate()
    {

        if (Random.Range(0, 100f) > 99.9f) { lowChanceFlip = !lowChanceFlip; roamMode = Random.Range(0, 2); }
        if (Random.Range(0, 100f) > 90f) { lowChanceFlip2 *= -1; }
        DoGroundAI();
        if (transform.position.y < -100)
        {
            Die();
        }
    }
    void DoGroundAI()
    {
        //DROP TO 0
        if (yRotation > 0)
            yRotation -= yRotationReturn;
        if (yRotation < 0)
            yRotation += yRotationReturn;
        if (yRotation >= 360)
            yRotation -= 360;
        if (yRotation <= -360)
            yRotation += 360;

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
        if (hasFoundPlayer)
        {
            if (Random.Range(0, 100) > pointAtPlayerChance)
            {
                //transform.eulerAngles = new Vector3(0, pointer.transform.eulerAngles.y, 0);
            }
        }
        //move forward / back up and rotate, depending on sensors
        else if (roamMode==0)
        {
            //sensor mode
            if (!s)
            {
                transform.Translate(Vector3.forward * acceleration);
            }
            else
            {
                yRotation += Random.Range(-wanderRotationLimits, wanderRotationLimits);
            }
        }
        else if (roamMode == 1)
        {
            if(Random.Range(0,100f)>98f)
                yRotation = Random.Range(-3, 3);
        }

        if (s)
        {
            //backup if stuck
            transform.Translate(-Vector3.forward * acceleration * Random.Range(reverseModifier.x, reverseModifier.y));
            yRotation += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
        }
        //set final rotation
        transform.Rotate(0, yRotation, 0);
    }
}
