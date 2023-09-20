using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AerialEnemy : Enemy
{
    AnimationCurve BaseCurve=new AnimationCurve();
    public List<Vector3> currentAttackCurve=null;
    public int frame = 0;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        if (RandomValues)
            GetRandomAIValues();
        transform.position = new Vector3(transform.position.x, EnemyManager.instance.aerialEnemyHeight, transform.position.z);
        GetComponent<SphereCollider>().radius = detectionRadius;
        pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        BaseCurve.AddKey(0, 0);
        BaseCurve.AddKey(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFoundPlayer)
        {
            Debug.DrawLine(transform.position, PlayerControllerTest.instance.transform.position, Color.yellow);
        }
        rayPointArmLeft.localEulerAngles = new Vector3(0, -lrRayAngle, 0);
        rayPointArmRight.localEulerAngles = new Vector3(0, lrRayAngle, 0);
        time += Time.deltaTime;
        //found player timer
        if (hasDetectedPlayer && !hasFoundPlayer && time > firstDetectedTime + timeDetectionToFind)
        {
            hasFoundPlayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            currentAttackCurve.Clear();
            frame = 0;
            currentAttackCurve = CalculateAttackCurve(BaseCurve, transform.position, (transform.position + transform.forward * 10), 30);
        }
        //when player is found, if close enough do damage
        if (hasFoundPlayer)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
            {
                if (hit.collider.gameObject == PlayerControllerTest.instance.gameObject)
                {
                    Debug.Log("Player contact");
                    anim.SetTrigger("Action");
                }
            }
        }

    }
    private void FixedUpdate()
    {
        DoRandomFlips();
        if (transform.position.y < -100)
        {
            Die();
        }
        if (hasFoundPlayer && currentAttackCurve.Count < 1 && Random.Range(0, 100f) > 95)
        {
            Vector3 v = new Vector3(PlayerControllerTest.instance.transform.position.x, transform.position.y, PlayerControllerTest.instance.transform.position.z);
            transform.LookAt(v);
            currentAttackCurve.Clear();
            frame = 0;
            currentAttackCurve = CalculateAttackCurve(BaseCurve, transform.position, (transform.position + transform.forward * (2*Vector3.Distance(transform.position,v))), 30);
        }
        //if there is an attack curve
        if (currentAttackCurve.Count >0)
        {
            if (frame < currentAttackCurve.Count)
            {
                Debug.DrawLine(transform.position, currentAttackCurve[frame], Color.magenta, 5f);

                transform.position = currentAttackCurve[frame];
                frame++;
            }
            else
                currentAttackCurve.Clear();
        }
        else
        {
            DoAerialAI();
            //PlayerTracking();
        }
    }
    void DoAerialAI()
    {
        //DROP TO 0
        if (yRotation > 0)
            yRotation = Mathf.Clamp(yRotation - yRotationReturn, 0, 360);
        if (yRotation < 0)
            yRotation = Mathf.Clamp(yRotation + yRotationReturn, -360, 0);

        //WANDER
        if (lowChanceFlip)
            transform.Translate(Vector3.forward * acceleration * Random.Range(0, wanderForceLimits));
        else
            transform.Translate(Vector3.right * acceleration * Random.Range(0, wanderForceLimits) * lowChanceFlip2);

        //PLAYER HUNT BEHAVIOR

        //when flying, if close to player, lerp y offset.

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
        /*if (!s && !(l || r))
        {
            //proceed forward
            transform.Translate(Vector3.forward * acceleration);
        }*/
        if (s)
        {
            //backup if stuck
            transform.Translate(-Vector3.forward * acceleration * Random.Range(reverseModifier.x, reverseModifier.y));
            yRotation += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
        }
        //set final rotation
        transform.Rotate(0, yRotation, 0);
    }


    static List<Vector3> CalculateAttackCurve(AnimationCurve baseCurve, Vector3 origin, Vector3 target, float steps)
    {
        AnimationCurve curve = new AnimationCurve();
        for(int b=0;b<baseCurve.keys.Length; b++)
        {
            curve.AddKey(baseCurve.keys[b]);
        }
        curve.AddKey(0.5f, -(origin.y-(PlayerControllerTest.instance.transform.position.y+1)));
        List<Vector3> output=new List<Vector3>();
        float slopeMod = 1 / steps;
        int i = 0;
        for (i=1; i< steps; i++)
        {
            output.Add((Vector3.Lerp(origin, target, (1/steps)*i)) + (Vector3.up*(curve.Evaluate(slopeMod * i))));
        }
        output.Add(new Vector3(target.x,origin.y,target.z));
        return output;
    }
    void DoRandomFlips()
    {
        if (Random.Range(0, 100f) > 99.5f) { lowChanceFlip = !lowChanceFlip; }
        if (Random.Range(0, 100f) > 90f) { lowChanceFlip2 *= -1; }
    }
    void PlayerTracking()
    {
        float offset = 0;
        if (hasFoundPlayer)
        {
            float dist = Mathf.Abs(Vector3.Distance(transform.position, PlayerControllerTest.instance.transform.position));
            offset = Mathf.Clamp(Map(detectionRadius - dist, dist, detectionRadius, 0, EnemyManager.instance.aerialEnemyHeight - EnemyManager.instance.groundEnemyHeight), 0, EnemyManager.instance.aerialEnemyHeight - EnemyManager.instance.groundEnemyHeight);
        }
        transform.position = new Vector3(transform.position.x, EnemyManager.instance.aerialEnemyHeight - offset, transform.position.z);
    }
}
