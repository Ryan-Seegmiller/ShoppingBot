using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamemanager;
using enemymanager;

public class RoverController : EnemyBase
{
    public int roamMode = 0;
    public float pounceForwardForce = 15;
    public float pounceUpForce = 15;
    public float pounceTorque = 15;
    bool canAttack = true;
    float attackStartTime=0;
    public Animator anim;
    public ParticleSystem dustEffect;
    public bool fleeing = false;
    Vector3 fleeVector;
    float fleeBounds = 5;
    float fleeBeginTime = 0;
    public void Start()
    {

        anim = GetComponentInChildren<Animator>();
        roamMode = Random.Range(0, 2);
        health = Random.Range(3, 6);
        startHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > fleeBeginTime + 5)
            fleeing = false;
        if (time>attackStartTime+3 && !canAttack)
            canAttack = true;
        if (!hasFoundPlayer && anim.GetBool("Action"))
            anim.SetBool("Action", false);
        if (fleeing)
            Debug.DrawLine(transform.position, fleeVector);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //take money from player
        if (collision.transform.gameObject.tag == "Player")
        {
            aS.PlayOneShot(attackAudio[Random.Range(0, attackAudio.Count)]);
            Flee();
            int cashToTake = Random.Range(5, 10);
            if (GameManager.Instance != null)
                GameManager.Instance.RemoveCash(cashToTake);
            Debug.Log(gameObject.name + " took " + cashToTake + " from player.");
        }
    }
    private void FixedUpdate()
    {
        base.FixedUpdate();
        if (Random.Range(0, 100f) > 99.9f && !fleeing){ roamMode = Random.Range(0, 2); }

        //run away to random spot until close enough
        if (fleeing)
        {
            transform.Translate(transform.forward * acceleration);
            if (transform.position.x < fleeVector.x + 3 && transform.position.x > fleeVector.x - 3 && transform.position.y < fleeVector.y + 3 && transform.position.y > fleeVector.y - 3)
                fleeing = false;
        }//main attack at player
        if (hasFoundPlayer && !fleeing)
        {
            //give it time to rotate
            if(anim.GetBool("Action"))
            anim.SetBool("Action", true);
            if (canAttack)
            {
                dustEffect.Play();
                transform.LookAt(new Vector3(PlayerControllerTest.instance.transform.position.x,transform.position.y,PlayerControllerTest.instance.transform.position.z));
                //pounce player
                canAttack = false;
                attackStartTime = time;
                rb.AddForce(transform.forward * pounceForwardForce * (Mathf.Abs(Vector3.Distance(transform.position, PlayerControllerTest.instance.transform.position))*8));
                rb.AddForce(Vector3.up * pounceUpForce);
            }
        }      
        DoRoamAI();
        DropAndClampTargetYRot();
        transform.Rotate(0, targetRotationY, 0);
    }
    void DropAndClampTargetYRot()
    {
        if (targetRotationY > 0)
            targetRotationY -= yRotationReturn;
        if (targetRotationY < 0)
            targetRotationY += yRotationReturn;
        if (targetRotationY >= 360)
            targetRotationY -= 360;
        if (targetRotationY <= -360)
            targetRotationY += 360;
    }
    void Flee()
    {
        fleeBeginTime = time;
        fleeing = true;
        roamMode = 0;
        fleeVector = transform.position+new Vector3(Random.Range(-fleeBounds, fleeBounds), transform.position.y, Random.Range(-fleeBounds, fleeBounds));
        transform.LookAt(fleeVector);
    }
    void DoRoamAI()
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
        //rotate if arms have collided
        if (r)
            targetRotationY += yRotationPerArmDetection;
        if (l)
            targetRotationY -= yRotationPerArmDetection;

        //move forward / back up and rotate, depending on sensors
        else if (roamMode==0)
        {
            //use sensors in this mode mode
            if (!s)
            {
                rb.AddForce(transform.forward * acceleration);
            }
            else
            {
                targetRotationY += Random.Range(-wanderRotationLimits, wanderRotationLimits);
            }
        }
        else if (roamMode == 1)
        {//random move mode
            if(Random.Range(0,100f)>98f)
                targetRotationY = Random.Range(-1, 1);
            if (Random.Range(0, 100f) > 35f)
                rb.AddForce(transform.forward * acceleration);
        }

        if (s)
        {
            //backup if stuck
            rb.AddForce(-transform.forward * acceleration * Random.Range(reverseModifier.x, reverseModifier.y));
            targetRotationY += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
        }
    }
}
