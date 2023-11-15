using UnityEngine;
using Items;
using enemymanager;

public class RoverController : EnemyBase
{
    private float lastAttackTime = 0;
    public void Start()
    {
        health = Random.Range(3, 6);
    }
    private new void FixedUpdate()
    {
        bool paused = EnemyManager.instance.PauseEnemies;
        if (!paused)
        {
            if (paused != wasPausedLastFrame)//if the player just unpaused, reset vel to cached value
            {
                UnPausePhysics();
            }
            base.FixedUpdate();
            if (hasFoundPlayer && time > lastAttackTime + 2)
            {
                if (Random.Range(0, 100) > 80)
                    transform.LookAt(new Vector3(player.transform.position.x + Random.Range(-1f, 1f), transform.position.y, player.transform.position.z + Random.Range(-1f, 1f)));
                rb.AddForce(transform.forward * acceleration * 1.5f);
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
                DoWander();
            }
        }
        else if (paused != wasPausedLastFrame)//if the player just paused for the first time, store its velocity, and set to 0
        {
            PausePhysics();
        }
        wasPausedLastFrame = paused;
    }
}
