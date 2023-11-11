using System.Collections;
using enemymanager;
using UnityEngine;
public class CrawlerController : EnemyBase
{
    public int roamMode = 0;
    public float ramForce = 15;
    public ParticleSystem explosion;
    public float explosionRadius = 5f;
    public Vector2 explosionForceRange = new Vector2(25, 100);
    public float explosionTorqueRange = 25;
    private void Start()
    {
        health = 2;
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (Random.Range(0, 100f) > 99.9f) {roamMode = Random.Range(0, 2); }

        if (hasFoundPlayer)
        {
            rb.AddForce(transform.forward * ramForce);
            if (Random.Range(0, 100f) > 90f)
            {
                transform.LookAt(player.transform.position);
                Vector3 loc = transform.localEulerAngles;
                transform.localEulerAngles = new Vector3(0, loc.y,loc.z);
            }
        }
        else
        {
            DoRoamAI();
        }
        if (hasFoundPlayer && currentDistanceToPlayer<1.3f)
        {
            Die();
            player.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
        }
        transform.Rotate(0, targetRotationY, 0);
    }


    private void DoRoamAI()
    {
        //SENSORS
        //arms/sensor rays
        rayBools = DoRays();
        //rotate if arms have collided
        if (rayBools[2])
            targetRotationY += yRotationPerArmDetection;
        if (rayBools[0])
            targetRotationY -= yRotationPerArmDetection;

        //move forward / back up and rotate, depending on sensors
        else if (roamMode==0)
        {
            //use sensors in this mode
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
        {//random rotate mode
            if(Random.Range(0,100f)>98f)
                targetRotationY = Random.Range(-3, 3);
        }

        if (rayBools[1])
        {
            //backup if stuck
            rb.AddForce(-transform.forward * acceleration * Random.Range(reverseModifierMinMax.x, reverseModifierMinMax.y));
            targetRotationY += Random.Range(stuckRotationMinMax.x, stuckRotationMinMax.y) * Random.Range(-1, 2);
        }

    }
    public void explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Rigidbody currentRb;
            if ((hitCollider.gameObject.tag == "Item" || hitCollider.gameObject.tag == "Player") && hitCollider.gameObject.TryGetComponent<Rigidbody>(out currentRb))
            {
                //enable items hit by explosion, add force and torque
                currentRb.isKinematic = false;
                currentRb.AddExplosionForce(Random.Range(explosionForceRange.x, explosionForceRange.y), transform.position, explosionRadius);
                currentRb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Random.Range(-explosionTorqueRange, explosionTorqueRange));
            }
        }
        explosion.gameObject.transform.parent = null;
        explosion.Play();
        Destroy(explosion,3);
    }
}
