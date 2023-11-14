using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;

public class RoverController : EnemyBase
{
    public int roamMode = 0;
    float lastAttackTime = 0;
    public void Start()
    {
        roamMode = Random.Range(0, 2);
        health = Random.Range(3, 6);
    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
        if (hasFoundPlayer && time > lastAttackTime + 2)
        {
            if(Random.Range(0,100)>80)
                transform.LookAt(new Vector3(player.transform.position.x + Random.Range(-1f,1f), transform.position.y, player.transform.position.z + Random.Range(-1f, 1f)));
            rb.AddForce(transform.forward * acceleration*1.5f);
            if (currentDistanceToPlayer < 2)//attack functions
            {
                anim.SetTrigger("action");
                lastAttackTime = time;
                if (ItemManager.instance != null)
                    ItemManager.instance.RemoveRandomItem();
                rb.AddForce(-transform.forward * acceleration * Random.Range(2f, 5f));
            }
        }
        else
        {
            DoRoamAI();
            transform.Rotate(0, targetRotationY, 0);
        }
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
                targetRotationY += Random.Range(-5f, 5f);
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
