using UnityEngine;
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
                rb.AddForce(-transform.forward * acceleration * Random.Range(2f, 15f));
            }
        }
        else
        {
            Wander();
        }
    }
}
