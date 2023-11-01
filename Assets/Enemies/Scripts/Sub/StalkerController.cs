using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;

public class StalkerController : EnemyBase
{
    AnimationCurve BaseCurve=new AnimationCurve();
    public List<Vector3> currentAttackCurve=null;
    public int frame = 0;
    public Animator anim;
    float lastAttackTime = 0;
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        BaseCurve.AddKey(0, 0);
        BaseCurve.AddKey(1, 0);
        health = 3;
    }
    new void FixedUpdate()
    {
        base.FixedUpdate();

        DropAndClampTargetYRot();

        //if found player, create attack curve
        if (hasFoundPlayer && currentAttackCurve.Count < 1 && lastAttackTime+3<time)
            BeginAttackCurve();
        //if there is an attack curve, run it
        if (currentAttackCurve.Count > 0)
            RunAttackCurvePhysics();
        else
        {
            DoAerialAI();
        }
        if (hasFoundPlayer)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
            {
                if (hit.collider.gameObject == player.gameObject)
                {
                    //take items from player
                    Debug.Log("Player contact");
                    anim.SetTrigger("Action");
                    if(ItemManager.instance!=null)
                        ItemManager.instance.RemoveRandomItem();
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
            rb.AddForce(transform.forward * acceleration * Random.Range(0, wanderForceLimits));
        else
            rb.AddForce(transform.right * acceleration * Random.Range(0, wanderForceLimits) * lowChanceFlip2);
        //rotate if arms have collided
        if (r)
            targetRotationY += yRotationPerArmDetection;
        if (l)
            targetRotationY -= yRotationPerArmDetection;
        if (s)
        {
            //backup if stuck
            rb.AddForce(-Vector3.forward * acceleration * Random.Range(reverseModifier.x, reverseModifier.y));
            targetRotationY += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
        }
        //set final rotation
        transform.Rotate(0, targetRotationY, 0);
    }
    void BeginAttackCurve()
    {
        //reset and ready curve list
        lastAttackTime = time;
        Vector3 v = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(v);
        currentAttackCurve.Clear();
        frame = 0;
        currentAttackCurve = CalculateAttackCurve(BaseCurve, transform.position, (transform.position + transform.forward * (2 * Vector3.Distance(transform.position, v))), 30, player.transform.position);
    }
    void RunAttackCurve()
    {
        //call repeatedly until currentAttackCurve clear, which happens at ELSE
        if (frame < currentAttackCurve.Count)
        {
            transform.position = currentAttackCurve[frame];
            frame++;
        }
        else
            currentAttackCurve.Clear();
    }    
    void RunAttackCurvePhysics()
    {
        //call repeatedly until currentAttackCurve clear, which happens at ELSE
        if (frame < currentAttackCurve.Count)
        {
            transform.LookAt(currentAttackCurve[frame]);
            rb.AddForce(transform.forward * 35);
            frame++;
        }
        else
            currentAttackCurve.Clear();
    }
    static List<Vector3> CalculateAttackCurve(AnimationCurve baseCurve, Vector3 origin, Vector3 target, float steps, Vector3 centerPoint)
    {
        //calculate a swooping curve between origin and target, using basecurve as the curve.
        AnimationCurve curve = new AnimationCurve();
        //base curve holds beggining and end keyframes for curve
        for(int b=0;b<baseCurve.keys.Length; b++)
        {
            curve.AddKey(baseCurve.keys[b]);
        }
        //add middle keyframe here for a negative slope.     ex. base keyframe at (0,0), new keyframe at (0.5,-5), base keyframe at (0,1). the new keyframe is in the middle(0.5f), and 5 down
        curve.AddKey(0.5f, -(origin.y-(centerPoint.y+1)));

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
}