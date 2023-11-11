using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;

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
        acceleration *= 2;
        roamMode = Random.Range(0, 2);
        health = Random.Range(3, 6);
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
            Flee();
            if (ItemManager.instance != null)
                ItemManager.instance.RemoveRandomItem();
        }
    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
        if (Random.Range(0, 100f) > 99.9f && !fleeing){ roamMode = Random.Range(0, 2); }

        //run away to random spot until close enough
        if (fleeing)
        {
            transform.Translate(transform.forward * acceleration);
            if ((transform.position.x < fleeVector.x + 3 && transform.position.x > fleeVector.x - 3 && transform.position.y < fleeVector.y + 3 && transform.position.y > fleeVector.y - 3)||time>fleeBeginTime+10)
                fleeing = false;
        }//main attack at player
        if (hasFoundPlayer && !fleeing)
        {
            //give it time to rotate
            if (canAttack)
            {
                dustEffect.Play();
                transform.LookAt(new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z));
                //pounce player
                canAttack = false;
                attackStartTime = time;
                rb.AddForce(transform.forward * pounceForwardForce * (Mathf.Abs(Vector3.Distance(transform.position, player.transform.position))*8));
                rb.AddForce(Vector3.up * pounceUpForce);
            }
        }      
        DoRoamAI();
        transform.Rotate(0, targetRotationY, 0);
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
        //rotate if arms have collided
        rayBools = DoRays();

        if (rayBools[2])
            targetRotationY += yRotationPerArmDetection;
        if (rayBools[0])
            targetRotationY -= yRotationPerArmDetection;

        //move forward / back up and rotate, depending on sensors
        else if (roamMode==0)
        {
            //use sensors in this mode mode
            if (!rayBools[1])
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

        if (rayBools[1])
        {
            //backup if stuck
            rb.AddForce(-transform.forward * acceleration * Random.Range(reverseModifierMinMax.x, reverseModifierMinMax.y));
            targetRotationY += Random.Range(stuckRotationMinMax.x, stuckRotationMinMax.y) * Random.Range(-1, 2);
        }
    }
}
