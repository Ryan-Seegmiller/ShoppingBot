using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StalkerController : EnemyBase
{
    AnimationCurve BaseCurve=new AnimationCurve();
    public List<Vector3> currentAttackCurve=null;
    public int frame = 0;
    public Animator anim;
    void Start()
    {
        if (GenerateRandomValues)
            GetRandomAIValues();
        transform.position = new Vector3(transform.position.x, EnemyManager.instance.aerialEnemyHeight, transform.position.z);
        GetComponent<SphereCollider>().radius = detectionRadius+5;//aerial enemy gets wider range
        pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        BaseCurve.AddKey(0, 0);
        BaseCurve.AddKey(1, 0);
        rayPointArmLeft.localEulerAngles = new Vector3(0, -lrRayAngle, 0);
        rayPointArmRight.localEulerAngles = new Vector3(0, lrRayAngle, 0);
        aS = GetComponentInChildren<AudioSource>();
        health = 3;

    }

    void Update()
    {
        if (hasFoundPlayer)
        {
            Debug.DrawLine(transform.position, PlayerControllerTest.instance.transform.position, Color.yellow);
        }

        time += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        DropAndClampTargetYRot();
        DoFlips();
        if (hasDetectedPlayer && !hasFoundPlayer && time > firstDetectedTime + timeDetectionToFind)
        {
            aS.PlayOneShot(detectedAudio[Random.Range(0, detectedAudio.Count)]);

            hasFoundPlayer = true;
        }
        if (transform.position.y < -100)
        {
            Die();
        }
        //if found player, create attack curve
        if (hasFoundPlayer && currentAttackCurve.Count < 1 && Random.Range(0, 100f) > 95)
        {
            BeginAttackCurve();
        }
        //if there is an attack curve, run it
        if (currentAttackCurve.Count >0)
        {
            RunAttackCurve();
        }
        else
        {
            DoAerialAI();
            //PlayerTracking();
        }
        if (hasFoundPlayer)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
            {
                if (hit.collider.gameObject == PlayerControllerTest.instance.gameObject)
                {
                    //take items from player
                    Debug.Log("Player contact");
                    anim.SetTrigger("Action");
                    if(GameManager.Instance!=null)
                        GameManager.Instance.RemoveRandomItem();
                }
            }
        }
    }
    void DropAndClampTargetYRot()
    {
        //clamp values and lerp back to 0
        if (targetRotationY > 0)
            targetRotationY -= yRotationReturn;
        if (targetRotationY < 0)
            targetRotationY += yRotationReturn;
        if (targetRotationY >= 360)
            targetRotationY -= 360;
        if (targetRotationY <= -360)
            targetRotationY += 360;
    }
    void DoAerialAI()
    {



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
        //WANDER
        if (lowChanceFlip && !s)
            transform.Translate(Vector3.forward * acceleration * Random.Range(0, wanderForceLimits));
        else
            transform.Translate(Vector3.right * acceleration * Random.Range(0, wanderForceLimits) * lowChanceFlip2);
        //rotate if arms have collided
        if (r)
            targetRotationY += yRotationPerArmDetection;
        if (l)
            targetRotationY -= yRotationPerArmDetection;

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
            targetRotationY += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
        }
        //set final rotation
        transform.Rotate(0, targetRotationY, 0);
    }
    void BeginAttackCurve()
    {
        //reset and ready curve list
        Vector3 v = new Vector3(PlayerControllerTest.instance.transform.position.x, transform.position.y, PlayerControllerTest.instance.transform.position.z);
        transform.LookAt(v);
        currentAttackCurve.Clear();
        frame = 0;
        currentAttackCurve = CalculateAttackCurve(BaseCurve, transform.position, (transform.position + transform.forward * (2 * Vector3.Distance(transform.position, v))), 30);
    }
    void RunAttackCurve()
    {
        //call repeatedly until currentAttackCurve clear, which happens at ELSE
        if (frame < currentAttackCurve.Count)
        {
            Debug.DrawLine(transform.position, currentAttackCurve[frame], Color.magenta, 5f);
            transform.position = currentAttackCurve[frame];
            frame++;
        }
        else
            currentAttackCurve.Clear();
    }
    static List<Vector3> CalculateAttackCurve(AnimationCurve baseCurve, Vector3 origin, Vector3 target, float steps)
    {
        //calculate a swooping curve between origin and target, using basecurve as the curve.
        AnimationCurve curve = new AnimationCurve();
        //base curve holds beggining and end keyframes for curve
        for(int b=0;b<baseCurve.keys.Length; b++)
        {
            curve.AddKey(baseCurve.keys[b]);
        }
        //add middle keyframe here for a negative slope.     ex. base keyframe at (0,0), new keyframe at (0.5,-5), base keyframe at (0,1). the new keyframe is in the middle(0.5f), and 5 down
        curve.AddKey(0.5f, -(origin.y-(PlayerControllerTest.instance.transform.position.y+1)));

        List<Vector3> output=new List<Vector3>();
        float slopeMod = 1 / steps;
        //with steps being the degree of accuracy between movements, fill a list of all the positions along determined curve
        int i = 0;
        for (i=1; i< steps; i++)
        {
            output.Add((Vector3.Lerp(origin, target, (1/steps)*i)) + (Vector3.up*(curve.Evaluate(slopeMod * i))));
        }
        //return list full of curve cordinates
        output.Add(new Vector3(target.x,origin.y,target.z));
        return output;
    }
    void DoFlips()
    {
        //random boolean flips used for AI
        if (Random.Range(0, 100f) > 95f) { lowChanceFlip = !lowChanceFlip; }
        if (Random.Range(0, 100f) > 99.9f) { lowChanceFlip2 *= -1; }
    }
    void PlayerTracking()
    {
        //while getting closer to the player, lerp closer to ground
        float offset = 0;
        if (hasFoundPlayer)
        {
            float dist = Mathf.Abs(Vector3.Distance(transform.position, PlayerControllerTest.instance.transform.position));
            offset = Mathf.Clamp(Map(detectionRadius - dist, dist, detectionRadius, 0, EnemyManager.instance.aerialEnemyHeight - EnemyManager.instance.groundEnemyHeight), 0, EnemyManager.instance.aerialEnemyHeight - EnemyManager.instance.groundEnemyHeight);
        }
        transform.position = new Vector3(transform.position.x, EnemyManager.instance.aerialEnemyHeight - offset, transform.position.z);
    }
}
